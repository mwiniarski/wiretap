#include "Device.h"

Device::Device(int socket_, sockaddr_in a_)
    :serializer(socket_, a_)
{}

void Device::operator()(){

    int uuid;
    try {
        uuid = serializer.acceptDevice();

    }catch(std::logic_error &e) {
        std::cout << e.what() << std::endl;
        return;
    }

    database.connect();
    if(!database.isAccepted(uuid)) {
        database.disconnect();
        return;
    }

    //TODO: Do bazy to ma sie zapisywać
    std::string file, ext;
    for(int i = 0 ; i < 1000; i++) {
        file = "file" + std::to_string(i);
        FILE *fp = nullptr;

        try {
            ext = serializer.getMessage(fp, file.c_str());
        } catch(std::logic_error &e) {
            fclose(fp);
            break;
        }
        fclose(fp);
        std::rename(file.c_str(),(file + ext).c_str());
    }

    database.disconnect();
}
