#include "DbManager.h"

DbManager::DbManager()
{}

std::string DbManager::generateName(int len) {
    std::string const default_chars =
    "abcdefghijklmnaoqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    std::mt19937_64 gen { std::random_device()() };
    std::uniform_int_distribution<size_t> dist { 0, default_chars.length()-1 };
    std::string ret;
    std::generate_n(std::back_inserter(ret), len, [&] { return default_chars[dist(gen)]; });
    return ret;
}

bool DbManager::isAccepted(int uuid, std::string deviceType)
{
    std::string sql = "SELECT * FROM blog_device "
                      "WHERE uuid = " + std::to_string(uuid);

    pqxx::work txn(*connection);
    pqxx::result query = txn.exec(sql);

    //record exists
    if(query.size() == 1) {
        std::string status = query.front()[3].as<std::string>();
        if(status == "active")
            return true;

        return false;
    }

    //new device
    sql = "INSERT INTO blog_device "
          "VALUES (DEFAULT, " +         //id
          std::to_string(uuid) + ", '" +//uuid
          deviceType +                  //device_type
          "', 'new', '" +                //status
          generateName(16) +            //name
          "', 10, 10)";                 //send_cycles

    std::cout << sql << std::endl;
    txn.exec(sql);
    txn.commit();

    return false;
}

bool DbManager::connect() {
    connection = std::make_unique<pqxx::connection>
                    (
                        "dbname = wiretap "
                        "user = master "
                        "password = master "
                        "hostaddr = 37.233.98.52 "
                        "port = 5432"
                    );

    return connection->is_open();
}

void DbManager::disconnect()
{
    if(connection->is_open())
        connection->disconnect();
}
