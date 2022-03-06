from django.urls import path
from rest_framework.authtoken.views import obtain_auth_token

from BackEnd.views import *

# path(route, view, kwargs=None, name=None)
urlpatterns = [
    path('', home, name='home'),
    path('register', register_user, name='register'),
    path('login', obtain_auth_token, name='login'),

    path('stock_search', stock_search, name='stock_search'),
    path('get_more_info', get_more_info, name='get_more_info'),

    path('follow_stock', follow_new_stock, name='follow_stock'),
    path('get_user_favourites_stocks', get_user_favourites_stocks, name='get_user_favourites_stocks'),

]
