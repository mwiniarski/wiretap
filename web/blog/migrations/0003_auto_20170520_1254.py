# -*- coding: utf-8 -*-
# Generated by Django 1.11.1 on 2017-05-20 10:54
from __future__ import unicode_literals

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('blog', '0002_config_device_file'),
    ]

    operations = [
        migrations.AlterField(
            model_name='device',
            name='configuration',
            field=models.ForeignKey(default=1, on_delete=django.db.models.deletion.CASCADE, to='blog.Config'),
        ),
    ]
