from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

class Post(models.Model):
    author = models.ForeignKey('auth.User')
    title = models.CharField(max_length=200)
    text = models.TextField()
    created_date = models.DateTimeField(
            default=timezone.now)
    published_date = models.DateTimeField(
            blank=True, null=True)

    def publish(self):
        self.published_date = timezone.now()
        self.save()

    def __str__(self):
        return self.title


class Device(models.Model):
    uuid = models.CharField(max_length = 20)
    device_type=models.CharField(max_length = 20)
    status=models.CharField(max_length = 20)
    name=models.CharField(max_length = 30)
    send_cycle_1 =  models.IntegerField()
    send_cycle_2 =  models.IntegerField()


class File(models.Model):
    FILE_TYPES=(
         ('A', 'Audio'),
         ('P', 'Picture'),
         ('T', 'Text'),)
    path = models.CharField(max_length = 100)
    timestamp = models.DateField()
    file_type=models.CharField(max_length=1,choices=FILE_TYPES)
    source=models.ForeignKey(Device,on_delete=models.CASCADE)
