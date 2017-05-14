#include "Manager.h"

Manager::Manager(short recvPort_, short sendPort_)
{
    port[RECV] = recvPort_;
    port[SEND] = sendPort_;
}

void Manager::passControl() {

}

void Manager::acceptIncomingConnections(int i) {
    int newSocket;
    sockaddr_in newAddress;
    socklen_t addrLength;

    while(true) {
        newSocket = accept(serverSocket[i], (sockaddr *) &newAddress, &addrLength);

        //conection is valid
        if (newSocket > 0) {

            std::cout << "Connection: " << inet_ntoa(newAddress.sin_addr) << std::endl;
            //use of operator() in Device
            Device newDevice(newSocket, newAddress);
            std::thread newThread(newDevice);

            if(i == (int)RECV) {
                devices.push_back(std::make_unique<Device> (newDevice));
            }
            newThread.detach();
        }
    }
}

void Manager::run() {

    std::unique_ptr<std::thread> t[SIZE];

    for(int i = 0; i < SIZE; i++) {
        //create TCP socket
        if ((serverSocket[i] = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
            throwError("Socket");
        }

        sockaddr_in serverAddress;
        //set port & allow any incoming address
        serverAddress.sin_family = AF_INET;
        serverAddress.sin_port = htons(port[i]);
        serverAddress.sin_addr.s_addr = INADDR_ANY;

        // reuse same port after disconnect
        int optval;
        setsockopt(serverSocket[i], SOL_SOCKET, SO_REUSEADDR, &optval, sizeof(optval));

        //bind socket
        if (bind(serverSocket[i], (sockaddr *) &serverAddress, sizeof(serverAddress)) < 0) {
            throwError("Bind");
        }

        //listen & set max connection number
        listen(serverSocket[i], 16);

        t[i] = std::make_unique<std::thread>(&Manager::acceptIncomingConnections, this, i);
    }

    passControl();

    t[0]->join();
    t[1]->join();
}

void Manager::throwError(std::string msg){
    throw std::runtime_error("Manager error on: " + msg );
}
