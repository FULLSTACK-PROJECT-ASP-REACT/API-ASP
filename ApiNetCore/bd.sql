create table tbl_category
(
    id_cat     int identity
        constraint tbl_category_pk
            primary key,
    name_cat   varchar(100) not null,
    status_cat char     default 'A',
    created_at datetime default getdate()
)
go

create table tbl_product
(
    id_pro          int identity
        constraint tbl_product_pk
            primary key nonclustered,
    code_pro        varchar(100)          not null,
    name_pro        varchar(max)          not null,
    description_pro text,
    price_unit_pro  decimal(10, 2)        not null,
    created_at      datetime default getdate(),
    update_at       datetime default getdate(),
    status_pro      char     default 'A',
    stock_pro       int      default 1000 not null,
    image_pro       varchar(max)
)
go

create table tbl_category_product
(
    id_ca_pr   int identity
        constraint tbl_category_product_pk
            primary key,
    pro_id     int
        constraint tbl_category_product_tbl_product_id_pro_fk
            references tbl_product,
    cat_id     int
        constraint tbl_category_product_tbl_category_id_cat_fk
            references tbl_category,
    created_at datetime default getdate()
)
go

create clustered index tbl_product_id_pro_index
    on tbl_product (id_pro)
go

create table tbl_transaction
(
    id_tra            int identity
        constraint tbl_transaction_pk
            primary key,
    emission_date_tra datetime default getdate(),
    type_tra          char not null,
    price_unit_tra    int  not null,
    total_amount_tra  int  not null,
    status_tra        char     default 'A',
    message_tra       text
)
go

create table tbl_detail_transaction
(
    id_d_t          int identity
        constraint tbl_detail_tran_pk
            primary key,
    pro_id          int
        constraint tbl_detail_tran_tbl_product_id_pro_fk
            references tbl_product,
    tra_id          int
        constraint tbl_detail_tran_tbl_transaction_id_tra_fk
            references tbl_transaction,
    code_stub       varchar(250),
    price_unit      decimal(10, 2) not null,
    subtotal        decimal(18, 2) not null,
    total           decimal(18, 2) not null,
    amount          int            not null,
    description_tra varchar(250)
)
go

