from rest_framework import serializers
from BackEnd.models import *


class RegistrationSerializer(serializers.ModelSerializer):
    # Fields that are not inside AppUser
    password_confirm = serializers.CharField(style={'input_type': 'password'}, write_only=True)

    class Meta:
        model = AppUser
        fields = ['email', 'name', 'surname', 'nickname', 'password', 'password_confirm']
        extra_kwargs = {
            'password': {'write_only': True}  # More security
        }

    # Override save method to make passwords match
    def save(self, **kwargs):
        app_user = AppUser(
            email=self.validated_data['email'],
            name=self.validated_data['name'],
            surname=self.validated_data['surname'],
            nickname=self.validated_data['nickname']
        )
        password = self.validated_data['password']
        password_confirm = self.validated_data['password_confirm']

        if password != password_confirm:
            raise serializers.ValidationError({'password': 'Password does not match'})
        app_user.set_password(password)
        app_user.save()
        return app_user


class StockSerializer(serializers.ModelSerializer):

    symbol = serializers.CharField(max_length=20, validators=[])

    class Meta:
        model = Stock
        fields = ['symbol', 'shortName', 'regularMarketPrice', 'regularMarketChange',
                  'regularMarketChangePercent', 'created_at', 'update_at']

        read_only_fields = ['created_at']

        # to use symbol in the url instead of the id by default, we use
        # lookup_fields = 'symbol'
        # extra_kwargs = {
        #     'url': {'lookup_fields': 'symbol'}
        # }


class FollowedStockSerializer(serializers.ModelSerializer):
    # SlugRelatedField may be used to represent the target of the relationship using a field on the target
    user = serializers.SlugRelatedField(
        many=False,
        read_only=False,
        slug_field='email',  # The field that uniquely identifies any given instance.
        queryset=AppUser.objects.all(),  # The queryset used for model instance lookups when validating the field input.
    )

    # stock_symbol = serializers.SlugRelatedField(
    #     many=False,
    #     read_only=False,
    #     slug_field='symbol',
    #     queryset=Stock.objects.all(),
    # )

    # PrimaryKeyRelatedField may be used to represent the target of the relationship using its primary key
    stock_symbol = serializers.PrimaryKeyRelatedField(
        many=False,
        read_only=False,
        queryset=Stock.objects.all()
    )

    class Meta:
        model = FollowedStock
        fields = ['user', 'stock_symbol']
        # read_only_fields = ['id']

    def get_unique_together_validators(self):
        # Determine a default set of validators for any unique_together constraints.

        """Overriding method to disable unique together checks """
        return []


class SearchCounterSerializer(serializers.ModelSerializer):

    symbol = serializers.CharField(max_length=20, validators=[])

    class Meta:
        model = SearchCounter
        fields = ['symbol', 'number_of_times_searched']

