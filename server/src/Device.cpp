#include "Device.h"

Device::Device(int socket_, sockaddr_in a_)
    :serializer(socket_, a_)
{}

std::string Device::getTimestamp()
{
    using namespace std::chrono;

    int ret = duration_cast<milliseconds>
    (system_clock::now().time_since_epoch()).count();

    return std::to_string(ret);
}

std::string getPath()
{
    std::ifstream config("../web/config.py");
    std::string line;

    while(std::getline(config, line)) {
        if(line.substr(0,10) == "FILES_ROOT") {
            int pos = line.find("'");
            int end = line.find_last_of("'");
            line = line.substr(pos+1,end-pos-1);
            break;
        }
    }

    return line;
}

void Device::operator()()
{
    int uuid = 0;
    try {
        uuid = serializer.acceptDevice();

    }catch(std::logic_error &e) {
        std::cout << e.what() << std::endl;
        return;
    }

    database.connect();
    id = database.isAccepted(uuid, "mobile");
    if(id == 0) {
        database.disconnect();
        return;
    }

    std::string path = getPath() + std::to_string(uuid) + "/";
    std::string file = getTimestamp();

    FILE * fp = nullptr;

    try {
        serializer.getMessage(fp, path, file);
    } catch(std::logic_error &e) {
        fclose(fp);
        std::remove((path + file).c_str());
        database.disconnect();
        return;
    }

    database.addFile(id, file);
    database.disconnect();
}
