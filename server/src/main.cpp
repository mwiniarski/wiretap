#include "Manager.h"
#include "DbManager.h"
#include <iostream>
using namespace std;

int main(int argc, char** argv) {

    if(argc != 2) {
        cout << "Wrong arguments!" << endl;
        return 0;
    }
    cout << "ASDASD" << endl;

    DbManager d;

    if(d.connect())
        cout << "CONNECTED TO DB" << endl;

    cout << d.isAccepted(111, "mobile") << endl;
    cout << d.isAccepted(123, "mobile") << endl;

    short port = atoi(argv[1]);

    Manager manager(port, port+1);
    manager.run();

    return 1;
}
