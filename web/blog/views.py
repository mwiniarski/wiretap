from django.shortcuts import render, get_object_or_404
from django.utils import timezone
from .models import Post
from .models import Device
from .models import File
from django.views.generic.edit import CreateView
from .forms import DeviceCreate
from django.views.generic.edit import UpdateView
from django.core.urlresolvers import reverse_lazy
from cpplib import hello
class DeviceUpdate(UpdateView):
    model = Device
    fields = '__all__'
    template_name_suffix = '_update_form'
    success_url = reverse_lazy('device_list')
def post_list(request):
    posts = Post.objects.all()
    return render(request, 'blog/post_list.html', {'posts': posts})
def post_detail(request, pk):
    post = get_object_or_404(Post, pk=pk)
    return render(request, 'blog/post_detail.html', {'post': post})
def device_info(request, pk):
    device = Device.objects.get(pk=pk)
    pictures = File.objects.filter(source_id=pk, file_type='P')
    textfiles = File.objects.filter(source_id=pk, file_type='T')
    return render(request, 'blog/device_info.html', {'device': device, 'pictures': pictures, 'textfiles': textfiles})
def device_list(request):
    new_devices = list(Device.objects.filter(status='new'))
    rest_devices = list(Device.objects.filter(status='active'))
    devices = new_devices + rest_devices
    lol = hello.greet()
    return render(request, 'blog/device_list.html', {'devices': devices, 'lol': lol})
def post_new(request):
    form = DeviceCreate()
    return render(request, 'blog/device_create.html', {'form': form})
def deactivate(request):
    pk = request.GET['test']
    Device.objects.filter(pk=pk).update(status='deactivated')
    device = Device.objects.get(pk=pk)
    pictures = File.objects.filter(source_id=pk, file_type='P')
    return render(request, 'blog/device_info.html', {'device': device, 'pictures': pictures})

def activate(request):
    pk = request.GET['test']
    Device.objects.filter(pk=pk).update(status='active')
    device = Device.objects.get(pk=pk)
    pictures = File.objects.filter(source_id=pk, file_type='P')
    textfiles = File.objects.filter(source_id=pk, file_type='T')
    return render(request, 'blog/device_info.html', {'device': device, 'pictures': pictures, 'textfiles': textfiles})


class AuthorCreate(CreateView):
    model = Device
    fields = '__all__'
    labels = {
        'Name': ('Nazwa'),
    }
