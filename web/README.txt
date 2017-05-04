Tworzenie bazy danych:

createuser --no-superuser --createdb --no-createrole testuser
createdb -O projects_user testdb
psql testdb -c "alter user testuser with encrypted password 'password';"

python manage.py migrate

Prawa admina:

python manage.py createsuperuser --username='admin' --email='a@b.c'
