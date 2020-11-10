#!/bin/bash

./make_style.sh
python3 manage.py collectstatic
python3 manage.py compilemessages
python3 manage.py compilejsi18n
python3 manage.py runserver 0.0.0.0:8000
