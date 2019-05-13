/*
 Navicat Premium Data Transfer

 Source Server         : aaemu
 Source Server Type    : MySQL
 Source Server Version : 80015
 Source Host           : localhost:3306
 Source Schema         : aikaemu_game

 Target Server Type    : MySQL
 Target Server Version : 80015
 File Encoding         : 65001

 Date: 13/05/2019 15:43:32
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for bank_gold
-- ----------------------------
DROP TABLE IF EXISTS `bank_gold`;
CREATE TABLE `bank_gold`  (
  `acc_id` int(10) UNSIGNED NOT NULL,
  `gold` bigint(20) UNSIGNED NOT NULL DEFAULT 0,
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`acc_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for character_friends
-- ----------------------------
DROP TABLE IF EXISTS `character_friends`;
CREATE TABLE `character_friends`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `char_id` int(10) UNSIGNED NOT NULL,
  `friend_id` int(10) UNSIGNED NOT NULL,
  `name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `is_blocked` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) ON UPDATE CURRENT_TIMESTAMP(0),
  `created_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`, `char_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for character_quests
-- ----------------------------
DROP TABLE IF EXISTS `character_quests`;
CREATE TABLE `character_quests`  (
  `char_id` int(10) UNSIGNED NOT NULL,
  `quest_id` smallint(5) UNSIGNED NOT NULL,
  `req_1` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `req_2` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `req_3` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `req_4` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `req_5` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `is_done` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `created_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`char_id`, `quest_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for character_skillbars
-- ----------------------------
DROP TABLE IF EXISTS `character_skillbars`;
CREATE TABLE `character_skillbars`  (
  `char_id` int(10) UNSIGNED NOT NULL,
  `slot` tinyint(3) UNSIGNED NOT NULL,
  `slot_id` smallint(5) UNSIGNED NOT NULL,
  `slot_type` tinyint(3) UNSIGNED NOT NULL,
  PRIMARY KEY (`char_id`, `slot`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for character_skills
-- ----------------------------
DROP TABLE IF EXISTS `character_skills`;
CREATE TABLE `character_skills`  (
  `char_id` int(10) UNSIGNED NOT NULL,
  `skill_id` smallint(5) UNSIGNED NOT NULL,
  `level` tinyint(3) UNSIGNED NOT NULL,
  PRIMARY KEY (`char_id`, `skill_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `acc_id` int(10) UNSIGNED NOT NULL,
  `slot` tinyint(3) UNSIGNED NOT NULL,
  `name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `level` smallint(5) UNSIGNED NOT NULL,
  `class` smallint(6) NOT NULL,
  `width` tinyint(4) UNSIGNED NOT NULL,
  `chest` tinyint(4) UNSIGNED NOT NULL,
  `leg` tinyint(4) UNSIGNED NOT NULL,
  `body` tinyint(4) UNSIGNED NOT NULL,
  `exp` bigint(20) UNSIGNED NOT NULL,
  `money` bigint(20) UNSIGNED NOT NULL,
  `skill_points` tinyint(4) UNSIGNED NOT NULL DEFAULT 0,
  `attr_points` tinyint(4) UNSIGNED NOT NULL DEFAULT 0,
  `hp` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `rotation` smallint(6) NOT NULL,
  `honor_point` int(11) NOT NULL,
  `pvp_point` int(11) NOT NULL,
  `infamy_point` int(11) NOT NULL,
  `str` tinyint(3) UNSIGNED NOT NULL,
  `agi` tinyint(3) UNSIGNED NOT NULL,
  `int` tinyint(4) UNSIGNED NOT NULL,
  `const` tinyint(3) UNSIGNED NOT NULL,
  `spi` tinyint(3) UNSIGNED NOT NULL,
  `token` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `created_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`, `acc_id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for items
-- ----------------------------
DROP TABLE IF EXISTS `items`;
CREATE TABLE `items`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `item_id` smallint(5) UNSIGNED NOT NULL,
  `acc_id` int(10) UNSIGNED NOT NULL,
  `char_id` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `pran_id` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `slot_type` tinyint(3) UNSIGNED NOT NULL,
  `slot` smallint(5) UNSIGNED NOT NULL,
  `effect1` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `effect2` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `effect3` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `effect1value` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `effect2value` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `effect3value` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `dur` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `dur_max` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `refinement` tinyint(3) UNSIGNED NOT NULL DEFAULT 0,
  `time` smallint(5) UNSIGNED NOT NULL DEFAULT 0,
  `created_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`, `acc_id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 30 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for prans
-- ----------------------------
DROP TABLE IF EXISTS `prans`;
CREATE TABLE `prans`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `acc_id` int(10) UNSIGNED NOT NULL,
  `char_id` int(10) UNSIGNED NOT NULL,
  `item_id` int(10) UNSIGNED NOT NULL,
  `name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `food` tinyint(4) NOT NULL,
  `devotion` int(11) NOT NULL,
  `p_cute` smallint(5) UNSIGNED NOT NULL,
  `p_smart` smallint(5) UNSIGNED NOT NULL,
  `p_sexy` smallint(5) UNSIGNED NOT NULL,
  `p_energetic` smallint(5) UNSIGNED NOT NULL,
  `p_tough` smallint(5) UNSIGNED NOT NULL,
  `p_corrupt` smallint(5) UNSIGNED NOT NULL,
  `level` smallint(5) UNSIGNED NOT NULL,
  `class` smallint(6) NOT NULL,
  `hp` int(10) UNSIGNED NOT NULL,
  `max_hp` int(10) UNSIGNED NOT NULL,
  `mp` int(10) UNSIGNED NOT NULL,
  `max_mp` int(10) UNSIGNED NOT NULL,
  `xp` int(10) UNSIGNED NOT NULL,
  `def_p` smallint(5) UNSIGNED NOT NULL,
  `def_m` smallint(5) UNSIGNED NOT NULL,
  `width` tinyint(3) UNSIGNED NOT NULL,
  `chest` tinyint(3) UNSIGNED NOT NULL,
  `leg` tinyint(3) UNSIGNED NOT NULL,
  `updated_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `created_at` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`, `acc_id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
