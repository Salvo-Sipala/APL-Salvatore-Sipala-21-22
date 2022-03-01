#include <iostream>
#include <map>
#include <set>
#include <string>

#include "SearchBar.cpp"
#include "DictCreation.cpp"
//#include "HttpServer.cpp"

#include <cpprest/http_listener.h>
#include <cpprest/json.h>

#define PRINT(msg)            wcout << msg
#define PRINT_ACTION(a, k, v) wcout << a << L" (" << k << L", " << v << L")\n"

using namespace web;
using namespace web::http;
using namespace web::http::experimental::listener;
using namespace std;
//using namespace httpServer;

map<utility::string_t, utility::string_t> requestDictionary;
map<utility::string_t, int> responseDictionary;


void display_json(json::value const& jvalue, utility::string_t const& prefix);
void handle_get(http_request request);
void handle_request(http_request request, function<void(json::value const&, json::value&)> action);
void handle_put(http_request request);


/****************MAIN**************/
int main()
{
	Button<int> b1;
	b1.printNumberOfInstance();

	Widget<int> w;
	w.printNumberOfInstance();

	SearchBar<int> s1, s2;
	s1.printNumberOfInstance();
	s2.printNumberOfInstance();

	Button<int> b2;
	b2.printNumberOfInstance();

	/****************LISTENER**************/

	http_listener listener(L"http://localhost:8082/stats/search_increment");

	//listener.support(methods::GET, handle_get);
	//listener.support(methods::POST, handle_post);
	listener.support(methods::PUT, handle_put);
	//listener.support(methods::DEL, handle_del);

	try
	{
		listener
			.open()
			.then([&listener]() {PRINT(L"\nstarting to listen\n"); })
			.wait();

		while (true);
	}
	catch (exception const& e)
	{
		wcout << e.what() << endl;
	}

	return EXIT_SUCCESS;

}

/****************FUNCTIONS**************/
void display_json(json::value const& jvalue, utility::string_t const& prefix)
{
	wcout << prefix << jvalue.serialize() << endl;
}

void handle_get(http_request request)
{
	PRINT(L"\nHandle GET\n");

	auto response = json::value::object(); // create an empty json object value

	for (auto const& p : requestDictionary)
	{
		response[p.first] = json::value::string(p.second);
	}

	display_json(json::value::null(), L"Request: ");
	display_json(response, L"Response: ");
	wcout << response << endl;
	request.reply(status_codes::OK, response);
}

void handle_request(http_request request, function<void(json::value const&, json::value&)> action)
{
	//empty json object
	auto response = json::value::array();

	request
		.extract_json()			// return pplx::task<json::value> task, input of the lambda
		.then([&response, &action](pplx::task<json::value> task) { // json::value is return type of task
		try
		{
			auto const& jvalue = task.get();
			display_json(jvalue, L"Request: ");

			if (!jvalue.is_null())
			{
				// jvalue must remain constant, so the reference have to be const
				action(jvalue, response);
			}
		}
		catch (http_exception const& e)
		{
			wcout << e.what() << endl;
		}
			})
		.wait();

			response = DictCreation::mapToJson(responseDictionary);
			display_json(response, L"Response: ");

			request.reply(status_codes::OK, response);
}

void handle_put(http_request request)
{
	PRINT("\nHandle PUT\n");
	wcout << "r.body:" << request.body() << endl;
	handle_request(
		request,
		[](json::value const& jvalue, json::value& response)
		{
			for (auto const& e : jvalue.as_object())
			{
				if (e.second.is_string())
				{
					auto key = e.second.as_string();

					if (responseDictionary.find(key) == responseDictionary.end()) // return iterator .end() if element not present
					{
						auto current_pair = responseDictionary.insert(pair<utility::string_t, int>(key, 0));
						auto value = responseDictionary[key];
						PRINT_ACTION(L"added", key, value);
						DictCreation::printMap(responseDictionary);
					}
					else
					{
						responseDictionary[key] = responseDictionary[key] + 1;
						auto value = responseDictionary[key];
						PRINT_ACTION(L"updated", key, value);
						DictCreation::printMap(responseDictionary);
					}

					wcout << "responseDictionary" << endl;
					DictCreation::printMap(responseDictionary);
				}
			}
		});
}
