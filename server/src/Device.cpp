#include "Device.h"

Device::Device(int socket_, sockaddr_in a_)
    :serializer(socket_, a_)
{}

void Device::operator()(){

    std::string file;
    for(int i=0;i < 1000; i++) {
        file = "file" + std::to_string(i);
        try {
            serializer.getMessage(file.c_str());
        } catch(std::logic_error &e) {
            break;
        }
    }

}
