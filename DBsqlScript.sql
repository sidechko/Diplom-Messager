SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

CREATE SCHEMA IF NOT EXISTS `mymessager` DEFAULT CHARACTER SET utf8 ;
USE `mymessager` ;

-- -----------------------------------------------------
-- Table `mymessager`.`usersaccounts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`usersaccounts` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `login` VARCHAR(45) NOT NULL,
  `username` VARCHAR(45) NULL DEFAULT 'login',
  `userpassword` VARCHAR(45) NOT NULL,
  `createtime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `login_UNIQUE` (`login` ASC) INVISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mymessager`.`channels`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`channels` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `channelname` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mymessager`.`messages`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`messages` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `content` VARCHAR(2000) NOT NULL,
  `channel` INT NOT NULL,
  `sender` INT NOT NULL,
  `sendtime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `updatetime` DATETIME NULL,
  PRIMARY KEY (`id`),
  INDEX `sender_idx` (`sender` ASC) VISIBLE,
  INDEX `channel_idx` (`channel` ASC) VISIBLE,
  CONSTRAINT `fk_message_sender_id`
    FOREIGN KEY (`sender`)
    REFERENCES `mymessager`.`usersaccounts` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_message_channel_id`
    FOREIGN KEY (`channel`)
    REFERENCES `mymessager`.`channels` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mymessager`.`messageseditlogs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`messageseditlogs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `Messageid` INT NOT NULL,
  `Prevcontent` VARCHAR(2000) NULL,
  `ChangeAt` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `messageid_idx` (`Messageid` ASC) INVISIBLE,
  CONSTRAINT `fk_edited_message_id`
    FOREIGN KEY (`Messageid`)
    REFERENCES `mymessager`.`messages` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mymessager`.`userchannels`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`userchannels` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `userid` INT NOT NULL,
  `availablechannel` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `iduserchannels_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `fk_userid_idx` (`userid` ASC) VISIBLE,
  INDEX `fk_availablechannel_channelid_idx` (`availablechannel` ASC) VISIBLE,
  CONSTRAINT `fk_userid_accountid`
    FOREIGN KEY (`userid`)
    REFERENCES `mymessager`.`usersaccounts` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_availablechannel_channelid`
    FOREIGN KEY (`availablechannel`)
    REFERENCES `mymessager`.`channels` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mymessager`.`requetslogs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mymessager`.`requetslogs` (
  `id` INT NOT NULL,
  `request` VARCHAR(512) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

USE `mymessager`;

DELIMITER $$
USE `mymessager`$$
CREATE DEFINER = CURRENT_USER TRIGGER `mymessager`.`messages_AFTER_UPDATE` AFTER UPDATE ON `messages` FOR EACH ROW
BEGIN
INSERT INTO messageseditlogs(Messageid, Prevcontent, ChangeAt) VALUE (NEW.id, OLD.content, current_time());
END$$


DELIMITER ;
CREATE USER 'serverUser' idENTIFIED BY 's1pepega';

GRANT ALL ON `mymessager`.* TO 'serverUser';
GRANT SELECT, INSERT, TRIGGER, UPDATE, DELETE ON TABLE `mymessager`.* TO 'serverUser';
GRANT SELECT ON TABLE `mymessager`.* TO 'serverUser';
GRANT SELECT, INSERT, TRIGGER ON TABLE `mymessager`.* TO 'serverUser';

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
