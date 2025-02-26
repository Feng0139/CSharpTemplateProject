ALTER TABLE `message_log` MODIFY COLUMN `id` CHAR(36) NOT NULL;
ALTER TABLE `message_log` CHANGE COLUMN `created_date` `created_at` datetime(3) NOT NULL;