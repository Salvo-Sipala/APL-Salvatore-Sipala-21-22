from rest_framework import routers

from BackEnd.viewset import StockViewSet

# Create a router and register our viewsets with it.
router = routers.DefaultRouter()
router.register('stocks', StockViewSet, basename='stocks')
