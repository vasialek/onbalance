alter table balance_item add size_name varchar(32) not null default('');

alter table balance_item add price_release decimal(10,4) not null default(0);

alter table balance_item add changed_from char(1) not null default('L');
