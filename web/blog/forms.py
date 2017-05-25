from django import forms
from django.views.generic.edit import CreateView
from .models import Device

class ContactForm(forms.Form):
    name = forms.CharField()
    message = forms.CharField(widget=forms.Textarea)

    def send_email(self):
        # send email using the self.cleaned_data dictionary
        pass


class DeviceCreate(forms.Form):
    model = Device
    fields = ['name']
    content = forms.CharField(max_length=256)
    created_at = forms.DateTimeField()

class PostForm(forms.Form):
    content = forms.CharField(max_length=256)
    created_at = forms.DateTimeField()
