#include "../Connection.h"
#include <sstream>

#include <boost/test/unit_test.hpp>
BOOST_AUTO_TEST_SUITE(ConnectionTests)

BOOST_AUTO_TEST_CASE(connection_constructor_works)
{
    sockaddr_in address;
    address.sin_port = htons(1234);
    Connection c(1, address);
    BOOST_CHECK_EQUAL(ntohs(c.getAddress().sin_port), 1234);
}

BOOST_AUTO_TEST_SUITE_END()
