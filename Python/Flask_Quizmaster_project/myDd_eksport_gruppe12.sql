-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: db
-- Generation Time: 02. Jun, 2023 13:30 PM
-- Tjener-versjon: 10.11.2-MariaDB-1:10.11.2+maria~ubu2204
-- PHP Version: 8.1.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `myDb`
--

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `alembic_version`
--

CREATE TABLE `alembic_version` (
  `version_num` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `alembic_version`
--

INSERT INTO `alembic_version` (`version_num`) VALUES
('04700f785928');

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `category`
--

CREATE TABLE `category` (
  `id` int(11) NOT NULL,
  `definition` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `category`
--

INSERT INTO `category` (`id`, `definition`) VALUES
(1, 'Land'),
(2, 'Monarker'),
(3, 'Geografi'),
(4, 'Musikk'),
(5, 'Naturvitenskap'),
(6, 'Dyr'),
(7, 'Underholdning'),
(8, 'Verden'),
(9, 'Sport'),
(10, 'Media'),
(11, 'Livsstil'),
(12, 'Kultur'),
(13, 'Historie'),
(14, 'Diverse');

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `quiz_question`
--

CREATE TABLE `quiz_question` (
  `id` int(11) NOT NULL,
  `definition` varchar(200) DEFAULT NULL,
  `type_id` int(11) NOT NULL,
  `active` int(11) DEFAULT 1,
  `category_id` int(11) NOT NULL,
  `alt1` varchar(200) NOT NULL,
  `alt2` varchar(200) DEFAULT NULL,
  `alt3` varchar(200) DEFAULT NULL,
  `alt4` varchar(200) DEFAULT NULL,
  `alt5` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `quiz_question`
--

INSERT INTO `quiz_question` (`id`, `definition`, `type_id`, `active`, `category_id`, `alt1`, `alt2`, `alt3`, `alt4`, `alt5`) VALUES
(1, 'Hva er hovedstaden i Irland???', 3, 1, 1, 'Dublin', 'London', 'Liverpool', 'Dubai', NULL),
(12, 'adaffa', 1, 0, 1, '', '', '', '', ''),
(13, 'Spørsmål 2', 2, 0, 5, 'Oslo', '', '', '', ''),
(16, 'dadadadaad', 1, 0, 1, '', '', '', '', ''),
(17, 'dadadada', 1, 0, 1, 'dadada', '', '', '', ''),
(18, 'afasfasfafaqeqe', 1, 0, 1, '', '', '', '', ''),
(19, NULL, 1, 0, 1, 'Oslo', '', '', '', ''),
(20, NULL, 2, 0, 3, '', '', '', '', ''),
(21, '11111111111111111111111', 2, 0, 13, 'afdaafasfs3244214', '', '', '', ''),
(22, 'ffffffffffffffffffffff', 1, 0, 1, 'dadadad', '', '', '', ''),
(23, 'Koko?', 1, 1, 1, 'Fleskerud', '', '', '', ''),
(24, 'mammamia', 2, 1, 13, 'gggggggg', '', '', '', ''),
(25, 'Halleluja', 3, 0, 12, 'Amanda', '', '', '', ''),
(26, 'LILL OG SILJE ER BEST - Ingen protest', 1, 1, 1, 'YESSDA', 'Berlin', 'qq', '', ''),
(27, 'Tralalallalalala ', 1, 0, 1, '', '', '', '', ''),
(28, 'Test spørsmål', 1, 0, 1, '', '', '', '', ''),
(29, 'Ja, vi elsker dette faget som det stiger freeeeem!!!!', 1, 1, 13, 'adadadd', 'Berlin', 'Kakemonster', '', ''),
(30, 'yredgtdsgsdg!!!!111112222', 2, 0, 14, 'sfsfs222', '222', '222', '222', '22'),
(31, 'adadada112132452352', 2, 1, 14, 'adad', 'adadadd', '', '', ''),
(32, 'HSkjhafkasjhfskjf', 2, 1, 13, 'sfsfsfsf', 'fsfssfs', '', '', ''),
(33, 'adaddad', 1, 0, 1, 'adada', '', '', '', '');

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `quiz_response`
--

CREATE TABLE `quiz_response` (
  `id` int(11) NOT NULL,
  `id_qst` int(11) NOT NULL,
  `id_user` int(11) NOT NULL,
  `quiz_answer` varchar(200) DEFAULT NULL,
  `quiz_status` int(11) DEFAULT NULL,
  `quiz_comment` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `quiz_response`
--

INSERT INTO `quiz_response` (`id`, `id_qst`, `id_user`, `quiz_answer`, `quiz_status`, `quiz_comment`) VALUES
(5, 1, 2, 'Your answer is deleted by the administrator', 2, 'tradadad'),
(6, 12, 2, 'Your answer is deleted by the administrator', 2, NULL),
(7, 13, 2, 'Your answer is deleted by the administrator', 2, NULL),
(10, 1, 1, 'Dubai', 2, 'For en flott by.'),
(11, 12, 1, 'adadada', 2, NULL),
(12, 13, 1, 'Roma', 2, NULL),
(15, 16, 2, 'Your answer is deleted by the administrator', 2, NULL),
(16, 17, 2, 'Your answer is deleted by the administrator', 2, NULL),
(17, 18, 2, 'Your answer is deleted by the administrator', 2, NULL),
(18, 19, 2, 'Your answer is deleted by the administrator', 2, NULL),
(19, 20, 2, 'Your answer is deleted by the administrator', 2, NULL),
(20, 21, 2, 'Your answer is deleted by the administrator', 2, NULL),
(21, 22, 2, 'Your answer is deleted by the administrator', 2, NULL),
(22, 23, 2, 'Your answer is deleted by the administrator', 2, 'fsfsfsf'),
(23, 24, 2, 'Your answer is deleted by the administrator', 2, 'sfsfsfs'),
(24, 25, 2, 'Your answer is deleted by the administrator', 2, NULL),
(25, 26, 2, 'Your answer is deleted by the administrator', 2, 'sfsfsfsfs'),
(26, 27, 2, 'Your answer is deleted by the administrator', 2, NULL),
(27, 28, 2, 'Your answer is deleted by the administrator', 2, 'sfsfsfss'),
(28, 29, 2, 'Your answer is deleted by the administrator', 2, 'fasfsfsfsfs'),
(29, 23, 1, ' ', 2, ''),
(30, 24, 1, 'gggggggg, ', 2, ''),
(31, 25, 1, 'Amanda', 2, NULL),
(32, 26, 1, ' ADADA', 2, ''),
(33, 29, 1, ' Ja, det er de.', 2, NULL),
(34, 28, 1, ' 8888888888888', 2, NULL),
(35, 30, 2, 'Your answer is deleted by the administrator', 2, NULL),
(36, 30, 1, ' ', 2, NULL),
(37, 31, 2, 'Your answer is deleted by the administrator', 2, ''),
(38, 32, 2, 'Your answer is deleted by the administrator', 2, ''),
(39, 31, 1, 'adad, adadadd', 2, NULL),
(40, 32, 1, 'sfsfsfsf, fsfssfs', 2, NULL),
(41, 33, 2, '', 0, NULL);

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `type`
--

CREATE TABLE `type` (
  `id` int(11) NOT NULL,
  `category` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `type`
--

INSERT INTO `type` (`id`, `category`) VALUES
(1, 'Essay/Text'),
(2, 'Flervalg'),
(3, 'Multiplechoise');

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `username` varchar(64) DEFAULT NULL,
  `firstname` varchar(64) DEFAULT NULL,
  `lastname` varchar(64) DEFAULT NULL,
  `password_hash` varchar(128) DEFAULT NULL,
  `isAdmin` smallint(6) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `user`
--

INSERT INTO `user` (`id`, `username`, `firstname`, `lastname`, `password_hash`, `isAdmin`) VALUES
(1, 'R0slin', 'Lill', 'Karlsen', 'pbkdf2:sha256:600000$bnCD6mVovuH66K88$810c03e58578367cfc92ae1756750cd40e428bbddbba20af0dafb92887a168db', 0),
(2, 'admin', 'admin', 'adminsen', 'pbkdf2:sha256:600000$7mq7ZTb5za0dksUO$bb48624115e71504452cb5a27a3727d24d7103a981238b6a5104d662de03508f', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `alembic_version`
--
ALTER TABLE `alembic_version`
  ADD PRIMARY KEY (`version_num`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `quiz_question`
--
ALTER TABLE `quiz_question`
  ADD PRIMARY KEY (`id`,`type_id`,`category_id`),
  ADD KEY `category_id` (`category_id`),
  ADD KEY `type_id` (`type_id`);

--
-- Indexes for table `quiz_response`
--
ALTER TABLE `quiz_response`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_qst` (`id_qst`),
  ADD KEY `id_user` (`id_user`);

--
-- Indexes for table `type`
--
ALTER TABLE `type`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ix_type_category` (`category`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ix_user_username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `quiz_question`
--
ALTER TABLE `quiz_question`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- AUTO_INCREMENT for table `quiz_response`
--
ALTER TABLE `quiz_response`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT for table `type`
--
ALTER TABLE `type`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Begrensninger for dumpede tabeller
--

--
-- Begrensninger for tabell `quiz_question`
--
ALTER TABLE `quiz_question`
  ADD CONSTRAINT `quiz_question_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `category` (`id`),
  ADD CONSTRAINT `quiz_question_ibfk_2` FOREIGN KEY (`type_id`) REFERENCES `type` (`id`);

--
-- Begrensninger for tabell `quiz_response`
--
ALTER TABLE `quiz_response`
  ADD CONSTRAINT `quiz_response_ibfk_1` FOREIGN KEY (`id_qst`) REFERENCES `quiz_question` (`id`),
  ADD CONSTRAINT `quiz_response_ibfk_2` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
