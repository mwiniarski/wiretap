#ifndef DBMANAGER_H_
#define DBMANAGER_H_
#include <pqxx/pqxx>
#include <random>
#include <algorithm>
#include <string>
#include <iostream>
#include <fstream>

class DbManager {
public:
    DbManager();
    ~DbManager(){delete connection;}

    int isAccepted(std::string uuid);
    std::string addFile(int id, std::string s, int file_type);
    bool connect();
    void disconnect();
private:
    void readConfigs();
    void throwError(std::string msg);
    std::string generateName(int uuid);
    pqxx::connection * connection;
    std::string configs[5];

    std::string deviceType;
};

#endif
