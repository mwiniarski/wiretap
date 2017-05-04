#include "Manager.h"
#include <iostream>

int main(int argc, char** argv) {

    if(argc != 2) {
        std::cout << "Wrong arguments!" << std::endl;
        return 0;
    }

    short port = atoi(argv[1]);

    Manager manager(port);
    manager.run();

    return 1;
}
