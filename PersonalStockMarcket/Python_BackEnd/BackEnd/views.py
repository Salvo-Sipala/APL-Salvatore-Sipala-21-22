from django.core.exceptions import ObjectDoesNotExist
from django.shortcuts import render
from django.http import JsonResponse
from djongo.exceptions import SQLDecodeError

from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework.authtoken.models import Token
from rest_framework.permissions import IsAuthenticated, AllowAny

from .models import FollowedStock
from .serializers import RegistrationSerializer, StockSerializer, FollowedStockSerializer
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


@api_view(['POST'])
@permission_classes([IsAuthenticated])
def stock_search(request):
    """
    search stock market with the symbol passed by front-end
    :param request: request from the front-end
    :return: response with data to the front-end
    """
    # increment_search_counter(request)
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
        # res = json.dumps(response_json)
        return Response(response_json, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Failed to search!'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
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

    followed_stock_serializer = FollowedStockSerializer(data=dict_followed_stock)
    if followed_stock_serializer.is_valid():
        if followed_stock_serializer.validated_data['user'] != request.user:
            return Response({'response': 'You have no permissions to create an observed product for somebody '
                                         'else!'}, status=status.HTTP_401_UNAUTHORIZED)
        else:
            try:
                followed_stock_serializer.save()
            except SQLDecodeError:
                return Response({'response': 'This element is already observed'}, status=status.HTTP_400_BAD_REQUEST)
    else:
        print(followed_stock_serializer.errors)
        return Response(followed_stock_serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    return Response('This is selected stock', status=status.HTTP_200_OK)


@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_user_favourites_stocks(request):
    """
    get all favourites stock for the user that make the request
    :param request: request from the front-end
    :return: response with data to the front-end
    """
    try:
        followed = FollowedStock.objects.filter(user=request.user.id)
        data = []
        for f in followed:
            stock = Stock.objects.get(id=f.stock_symbol)
            stock_serializer = StockSerializer(stock)
            data.append({
                "user": f.user.email,
                "product": stock_serializer.data,
            })
        return Response(data, status=status.HTTP_200_OK)
    except ObjectDoesNotExist:
        return Response({'response': 'There are no observations for this user at the moment'},
                        status=status.HTTP_400_BAD_REQUEST)


def increment_search_counter(request):
    headers = {
        'Content-Type': 'application/json',
    }

    url = "http://localhost:8081/stats/search_increment"

    data = {
        'symbol': request.data,
    }

    response = requests.post(url=url, headers=headers, data=data)

    if response.status_code == 200:
        json_data = json.loads(response.text)  # json_data is a dict, response.text is a string (in the shape of a dict)
        response_json = search_stock_mapping(json_data)
        # res = json.dumps(response_json)
        return Response(response_json, status=status.HTTP_200_OK)
    else:
        return Response({'response': 'Failed to search!'}, status=status.HTTP_400_BAD_REQUEST)
