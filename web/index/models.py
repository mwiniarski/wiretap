from django.db import models

class Device(models.Model):
    uuid = models.IntegerField()
    device_type=models.CharField(max_length = 20)
    status=models.CharField(max_length = 20)

class File(models.Model):
    path = models.CharField(max_length = 100)
    timestamp = models.CharField(max_length = 20)
    file_type = models.IntegerField()

class config(models.Model):
    time_period1=models.IntegerField()
    time_period2=models.IntegerField()

     
