# -*- coding: utf-8 -*-
# Generated by Django 1.11.1 on 2017-05-22 18:38
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('blog', '0004_device_name'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='device',
            name='configuration',
        ),
        migrations.AddField(
            model_name='device',
            name='send_cycle_1',
            field=models.IntegerField(default=1),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='device',
            name='send_cycle_2',
            field=models.IntegerField(default=1),
            preserve_default=False,
        ),
        migrations.DeleteModel(
            name='Config',
        ),
    ]