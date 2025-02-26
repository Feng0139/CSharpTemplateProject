create table if not exists `message_log`
(
    `id` VARCHAR(36) NOT NULL,
    `message_type` varchar(128) NOT NULL,
    `result_type` varchar(128) NOT NULL,
    `message_json` varchar(2048) NOT NULL,
    `result_json` varchar(2048) NOT NULL,
    `created_date` datetime(3) NOT NULL,
    PRIMARY KEY (`id`)
) charset=utf8mb4;