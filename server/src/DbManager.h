#ifndef DBMANAGER_H_
#define DBMANAGER_H_
#include <pqxx/pqxx>
#include <random>
#include <algorithm>
#include <string>
#include <iostream>

class DbManager {
public:
    DbManager();

    bool isAccepted(int uuid, std::string s);
    bool connect();
    void disconnect();
private:
    std::string generateName(int uuid);
    std::unique_ptr<pqxx::connection> connection;
};

#endif
