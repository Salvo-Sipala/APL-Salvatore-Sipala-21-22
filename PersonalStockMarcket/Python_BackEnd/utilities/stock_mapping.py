from django.db.migrations import serializer
from rest_framework.response import Response
from rest_framework import status

from BackEnd.models import Stock
from BackEnd.serializers import StockSerializer
from django.core.exceptions import ObjectDoesNotExist


def search_stock_mapping(json_data):
    """
    This def extract stock detail and pass to the response for the front-end (maybe i can save into db)
    :param json_data: a dict from yahoo finance api
    :return: another dict passed to response
    """
    dict_result = json_data["quoteResponse"]["result"][0]
    stock_list = []
    new_stock = Stock()
    new_stock.symbol = dict_result['symbol']
    if "shortName" in dict_result:
        new_stock.shortName = dict_result['shortName']
    else:
        print("shortName not present")
        new_stock.shortName = " "

    if "regularMarketPrice" in dict_result:
        new_stock.regularMarketPrice = dict_result['regularMarketPrice']
    else:
        print("regularMarketPrice not present")
        new_stock.regularMarketPrice = 0.00

    if "regularMarketChange" in dict_result:
        new_stock.regularMarketChange = dict_result['regularMarketChange']
    else:
        print("regularMarketChange not present")
        new_stock.regularMarketChange = 0.00

    if "regularMarketChangePercent" in dict_result:
        new_stock.regularMarketChangePercent = dict_result['regularMarketChangePercent']
    else:
        print("regularMarketChangePercent not present")
        new_stock.regularMarketChangePercent = 0.00

    # call save def
    try_to_save_stock(dict_result)
    stock_list.append(new_stock)
    print(stock_list)
    stock_serializer = StockSerializer(data=stock_list, many=True)  # serializer not valid

    if stock_serializer.is_valid():
        print("Serializer is valid!")
    # else:
    #     print("Serializer is NOT valid!")
    #     print(serializer.errors)
    # print(serializer.data)
    return stock_serializer.data


# without serializer
def stock_mapping(json_data):
    dict_result = json_data["quoteResponse"]["result"][0]
    dict_stock = {"symbol": dict_result["symbol"], "shortName": dict_result["shortName"],
                  "regularMarketPrice": dict_result["regularMarketPrice"],
                  "regularMarketChange": dict_result["regularMarketChange"],
                  "regularMarketChangePercent": dict_result["regularMarketChangePercent"]}
    # serializer = StockSerializer(data=dict_stock, many=True)  # serializer valid
    stock_list = [dict_stock]
    return stock_list


def try_to_save_stock(dict_result):
    try:
        print("enter try:")
        dict_stock = {"symbol": dict_result["symbol"], "shortName": dict_result["shortName"],
                      "regularMarketPrice": dict_result["regularMarketPrice"],
                      "regularMarketChange": dict_result["regularMarketChange"],
                      "regularMarketChangePercent": dict_result["regularMarketChangePercent"]}
        print(dict_stock)
        try:
            # stock already exist, so update it
            stock_selected = Stock.objects.get(symbol=dict_stock['symbol'])
            serializer_for_save = StockSerializer(instance=stock_selected, data=dict_stock)
            print('\n', serializer_for_save, serializer_for_save.is_valid())
            if serializer_for_save.is_valid():
                print("serializer valid(try)")
                serializer_for_save.save()
                print("full saved")
                return Response(serializer_for_save.data, status=status.HTTP_200_OK)
            else:
                print(serializer_for_save.errors)

        except ObjectDoesNotExist:
            # stock doesn't exist, so create it
            serializer_for_save = StockSerializer(data=dict_stock)
            print('\n', serializer_for_save, serializer_for_save.is_valid())
            if serializer_for_save.is_valid():
                print("serializer valid(try)")
                serializer_for_save.save()
                print("full saved")
                return Response(serializer_for_save.data, status=status.HTTP_200_OK)
            else:
                print(serializer_for_save.errors)

    except KeyError:
        print("enter except:")
        dict_stock = {"symbol": dict_result["symbol"], "shortName": "",
                      "regularMarketPrice": 0.00,
                      "regularMarketChange": 0.00,
                      "regularMarketChangePercent": 0.00}
        stock_selected = Stock.objects.get(symbol=dict_stock['symbol'])
        serializer_for_save = StockSerializer(instance=stock_selected, data=dict_stock)
        if serializer_for_save.is_valid():
            print("serializer valid(except)")
            serializer_for_save.save()
            print("not full saved")
