from django.db import models
# Imports used to generate a custom user
from django.contrib.auth.models import AbstractBaseUser, PermissionsMixin, BaseUserManager
from django.utils.translation import gettext_lazy as _
# Imports used to generate tokens
from django.conf import settings
from django.db.models.signals import post_save
from django.dispatch import receiver
from rest_framework.authtoken.models import Token


# Create your models here.
class CustomAccountManager(BaseUserManager):
    """
    Custom user model manager where email is the unique identifiers
    for authentication instead of usernames.
    """
    def create_user(self, email, name, surname, nickname, password, **other_fields):
        """
        Create and save a User with the given email and password.
        """
        # Validate email
        if not email:
            raise ValueError('Provide a correct email address')
        if not nickname:
            raise ValueError('Provide a nickname')
        # Lowercasing email
        email = self.normalize_email(email)
        user = self.model(email=email, name=name, surname=surname, nickname=nickname, **other_fields)
        user.set_password(password)
        user.save()
        return user

    def create_superuser(self, email, name, surname, nickname, password, **other_fields):
        """
        Create and save a SuperUser with the given email and password.
        """
        other_fields.setdefault('is_staff', True)
        other_fields.setdefault('is_superuser', True)
        # Validate other fields
        if other_fields.get('is_staff') is not True:
            raise ValueError('Superuser must have is_staff = True')
        if other_fields.get('is_superuser') is not True:
            raise ValueError('Superuser must have is_superuser = True')
        return self.create_user(email, name, surname, nickname, password, **other_fields)


class AppUser(AbstractBaseUser, PermissionsMixin):
    email = models.EmailField(_('email'), max_length=40, unique=True)  # _ -> email required
    name = models.CharField(max_length=40, null=False)
    surname = models.CharField(max_length=40, null=False)
    nickname = models.CharField(max_length=40, unique=True)
    is_staff = models.BooleanField(default=False)
    is_active = models.BooleanField(default=True)
    created_at = models.DateTimeField(auto_now_add=True)

    # Define the use of a custom account manager
    objects = CustomAccountManager()  # Specified that all objects for the class come from the CustomAccountManager

    USERNAME_FIELD = 'email'  # New primary key
    REQUIRED_FIELDS = ['nickname', 'name', 'surname']  # List of fields prompted when using createsuperuser

    def __str__(self):
        return "Nickname: " + self.nickname

    # For checking permissions: all admins have full permissions
    def has_perm(self, perm, obj=None):
        return self.is_staff

    # Easy mode: all users have permission to view this app
    def has_module_perms(self, app_label):
        return True


# This one will be called after a user has been saved
@receiver(post_save, sender=settings.AUTH_USER_MODEL)
def create_auth_token(sender, instance=None, created=False, **kwargs):
    if created:  # Each time a user is created, a token will be generated
        Token.objects.create(user=instance)


class Stock(models.Model):
    # id = models.AutoField(primary_key=True, default=0, null=False)
    symbol = models.CharField(primary_key=True, max_length=20, null=False, unique=True)
    shortName = models.CharField(max_length=100)
    regularMarketPrice = models.FloatField()
    regularMarketChange = models.FloatField()
    regularMarketChangePercent = models.FloatField()
    created_at = models.DateTimeField(auto_now_add=True)
    update_at = models.DateTimeField(auto_now=True)

    def __str__(self):
        return "Stock Symbol: " + self.symbol + ", Stock name: " + self.shortName


class FollowedStock(models.Model):
    user = models.ForeignKey('AppUser', on_delete=models.CASCADE)  # A many-to-one relationship.
    stock_symbol = models.ForeignKey('Stock', on_delete=models.CASCADE)
    # symbol = models.CharField(max_length=20, null=False, unique=True)

    class Meta:
        unique_together = ['user', 'stock_symbol']


class SearchCounter(models.Model):
    symbol = models.CharField(primary_key=True, max_length=20, null=False, unique=True)
    number_of_times_searched = models.IntegerField()

    # class Meta:
    #     unique_together = ['symbol', 'number_of_times_searched']
