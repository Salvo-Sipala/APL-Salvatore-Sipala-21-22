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

	// convert a std::map into json
	template<typename Map>
	static web::json::value mapToJson(Map& myMap)
	{
		web::json::value result = web::json::value::object();

		typename Map::iterator it;
		for (it = myMap.begin(); it != myMap.end(); it++)
		{
			result[(it->first)] = web::json::value(it->second);
		}
		return result;
	}

};
