from django.http import HttpResponse
# //from cpplib import hello

def index(request):
    return HttpResponse("Message from cpp file: ")
