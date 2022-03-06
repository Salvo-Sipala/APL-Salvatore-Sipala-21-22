from rest_framework import viewsets
from rest_framework.permissions import IsAdminUser

from BackEnd.models import Stock
from BackEnd.serializers import StockSerializer


# A viewset provides default create(), retrieve(), update(), partial_update(), destroy() and list() actions.
# ViewSets allows to combine ListView and DetailView into one view.
class StockViewSet(viewsets.ModelViewSet):
    queryset = Stock.objects.all()
    serializer_class = StockSerializer
    lookup_field = 'symbol'
    permission_classes = [IsAdminUser]
