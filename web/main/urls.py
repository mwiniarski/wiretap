from django.conf.urls import include, url
from django.contrib import admin
from blog import views

urlpatterns = [
    url(r'^admin/', admin.site.urls),
    url(r'^device/(?P<pk>[0-9]+)/$', views.device_info, name='xxx'),
    url(r'^wiretap', views.device_list, name='device_list'),
    url(r'^off/', views.deactivate, name='off'),
    url(r'^on/', views.activate, name='on'),
    url(r'^edit/(?P<pk>[0-9]+)/$', views.DeviceUpdate.as_view(), name='update_device'),
]
