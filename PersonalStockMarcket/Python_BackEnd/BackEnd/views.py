from django.core.exceptions import ObjectDoesNotExist
from django.db.utils import DatabaseError
from django.shortcuts import render
from django.http import JsonResponse
from djongo.exceptions import SQLDecodeError

from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.authtoken.models import Token
from rest_framework.permissions import IsAuthenticated, AllowAny

from BackEnd.models import AppUser, Stock, FollowedStock
from BackEnd.serializers import RegistrationSerializer, StockSerializer, FollowedStockSerializer
from utilities.stock_mapping import *

import json
import requests


# Create your views here.

# The @api_view decorator for working with function based views.
# api_view decorator takes a list of HTTP methods that your view should respond to
@api_view(['GET'])
@permission_classes([AllowAny])
def home(request):
    return Response('This is the home page!', status=status.HTTP_200_OK)


@api_view(['POST'])
@permission_classes([AllowAny])
def register_user(request):
    """
    User Registration
    :param request: receives a request from the front-end
    :return: json response with data to the front-end
    """
    serializer = RegistrationSerializer(data=request.data)  # request.data is a dict
    data = {}
    if serializer.is_valid():
        new_user = serializer.save()
        data['response'] = "Registration was successful"
        data['email'] = new_user.email
        data['name'] = new_user.name
        data['surname'] = new_user.surname
        data['nickname'] = new_user.nickname
        token = Token.objects.get(user=new_user).key
        data['token'] = token
        return JsonResponse(data, status=status.HTTP_201_CREATED)
    else:
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
@permission_classes([IsAuthenticated])
def stock_search(request):
    """
    search stock market with the symbol passed by front-end
    :param request: request from the front-end
    :return: response with data to the front-end
    """
    #increment_search_counter(request)
    # request_data = json.loads(request.body.decode('utf-8'))
    headers = {
        'Content-Type': 'application/json',
        'X-API-KEY': 'sED5TaN7LM7W763Hp5ux872XuqPM9AYS58iskZ6g'
    }

    yahoo_finance_api_url = 'https://yfapi.net/v6/finance/quote?region=US&lang=en&symbols=' + request.data

    response = requests.get(url=yahoo_finance_api_url, headers=headers)

    if response.status_code == 200:
        json_data = json.loads(response.text)  # json_data is a dict, response.text is a string (in the shape of a dict)
        response_json = search_stock_mapping(json_data)
        return Response(response_json, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Failed to search!'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['PUT'])
@permission_classes([IsAuthenticated])
def follow_new_stock(request):
    """
    follow new stock to observe it and add to favourites
    :param request: request from the front-end
    :return: response with data to the front-end
    """
    request_data = json.loads(request.body.decode('utf-8'))
    user_email = request_data["email"]
    followed_symbol = request_data["symbol"]

    dict_followed_stock = {
        'user': user_email,
        'stock_symbol': followed_symbol
    }

    try:
        # user that made follow request
        user_for_follow = AppUser.objects.get(email=dict_followed_stock['user'])
        # QuerySet that contains stock followed by user
        follow_stock_selected = FollowedStock.objects.filter(user_id=user_for_follow.id)
        # list comprehension of stock already followed
        symbol_list_followed = [f.stock_symbol_id for f in follow_stock_selected]

        if dict_followed_stock['stock_symbol'] in symbol_list_followed:
            return Response('This Stock is already followed!', status=status.HTTP_400_BAD_REQUEST)
        else:
            followed_stock_serializer = FollowedStockSerializer(data=dict_followed_stock)
            if followed_stock_serializer.is_valid():
                try:
                    followed_stock_serializer.save()
                    return Response(followed_stock_serializer.data, status=status.HTTP_200_OK)
                except DatabaseError:
                    return Response({'response': 'DatabaseError'}, status=status.HTTP_400_BAD_REQUEST)
            else:
                return Response(followed_stock_serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    except ObjectDoesNotExist:
        return Response({'response': 'This stock does not exist'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_favourites_stocks(request):
    """
    get all favourites stock for the user that make the request
    :param request: request from the front-end
    :return: response with data to the front-end
    """
    try:
        followed = FollowedStock.objects.filter(user_id=request.user.id)
        stock_list = []
        stock_serializer = StockSerializer()
        for f in followed:
            stock = Stock.objects.get(symbol=f.stock_symbol_id)
            stock_list.append(stock)
            stock_serializer = StockSerializer(data=stock_list, many=True)
            if stock_serializer.is_valid():
                print("Serializer is valid!")

        return Response(stock_serializer.data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


def increment_search_counter(request):
    headers = {
        'Content-Type': 'application/json'
    }

    url = "http://localhost:8082/stats/search_increment"

    data = {
        'symbol': request.data,
    }
    print(data)
    response = requests.put(url=url, headers=headers, data=data)
    print(response.text)
    if response.status_code == 200:
        json_data = json.loads(response.text)  # json_data is a dict, response.text is a string (in the shape of a dict)
        print(json_data)
    else:
        print(response.status_code)
