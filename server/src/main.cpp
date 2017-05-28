#include "Manager.h"
#include "DbManager.h"
#include <iostream>
#include <chrono>
#include <string>
#include <fstream>
using namespace std;

int main(int argc, char** argv) {

    if(argc != 2) {
        cout << "Wrong arguments!" << endl;
        return 0;
    }

    short port = atoi(argv[1]);

    Manager manager(port, port+1);
    manager.run();

    return 1;
}
