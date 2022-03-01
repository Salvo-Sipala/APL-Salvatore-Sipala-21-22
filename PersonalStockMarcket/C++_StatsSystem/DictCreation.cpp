#include <iostream>
#include <map>
#include <cpprest/json.h>

class DictCreation {
public:

	//void printMap (map<string, string> &m){};
	template<typename Map>
	static void printMap(Map& m) {
		std::wcout << "{ ";
		for (auto& item : m) {
			std::wcout << "'" << item.first << "':'" << item.second << "',\n\t";
		}
		std::wcout << " }" << std::endl;
	}

	template<typename Map>
	static web::json::value mapToJson(Map& myMap)
	{
		web::json::value result = web::json::value::array();
		//std::map<utility::string_t, int> myMap;

		int i = 0;
		for (std::pair<utility::string_t, int> p : myMap)
		{
			web::json::value obj = web::json::value::object();

			obj[U("symbol")] = web::json::value(p.first);
			obj[U("numberSerachedTimes")] = web::json::value(p.second);

			result[i++] = obj;
		}
		return result;
	}
};
