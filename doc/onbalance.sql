create table user_organization
(
    id integer primary key not null identity(20000, 1),
    username varchar(128) not null,
    organization_id integer not null
);

create table organization
(
    id integer primary key not null identity(500000, 1),
    status_id tinyint not null default(1),
    name varchar(255) not null,
    created_at datetime not null default(getdate()),
    updated_at datetime null
);

/* Point Of Service */
create table pos
(
    id integer primary key not null identity(90000, 1),
    status_id tinyint not null default(1),
    user_id varchar(128) not null,
    name varchar(128) not null,
    created_at datetime not null default(getdate())
);
create index PosUserIdIndex on pos(user_id);

create table product
(
    id integer primary key not null identity(100000, 1),
    status_id tinyint not null default(1),
    pos_id integer not null,
    uid varchar(64) not null unique,
    user_id varchar(128) not null,
    name varchar(128) not null,
    created_at datetime not null default(getdate())
);

create table product_detail
(
    id integer primary key not null identity(100000, 1),
    status_id tinyint not null default(1),
    product_id integer not null,

    parameter_name varchar(128) not null,
    parameter_value varchar(256) not null,

    price_minor decimal(10, 4) not null,
    price_release_minor decimal(10, 4) not null,
    quantity integer not null default(0),

    updated_at datetime not null,
    created_at datetime not null default(getdate())
);
create index ProductDetailParameterNameIndex on product_detail(parameter_name);
create index ProductDetailProductIdIndex on product_detail(product_id);

create table balance_item
(
    id integer primary key not null identity(100000, 1),
    status_id tinyint not null default(1),
    pos_id integer not null,
    internal_code varchar(64) not null,
    product_name varchar(128) not null,
    quantity integer not null default(0),
    price decimal(10,4) not null,
    is_new char(1) not null default('Y')
);
