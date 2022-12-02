-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: localhost    Database: ale
-- ------------------------------------------------------
-- Server version	8.0.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoria` (
  `idCategoria` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  `estado` varchar(45) DEFAULT 'habilitado',
  PRIMARY KEY (`idCategoria`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
INSERT INTO `categoria` VALUES (1,'Empanadas','habilitado'),(3,'Pastel','habilitado');
/*!40000 ALTER TABLE `categoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cliente`
--

DROP TABLE IF EXISTS `cliente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente` (
  `idCliente` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) DEFAULT NULL,
  `Telefono` varchar(45) DEFAULT NULL,
  `Direccion` varchar(45) DEFAULT NULL,
  `estado` varchar(90) DEFAULT 'habilitado',
  PRIMARY KEY (`idCliente`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente`
--

LOCK TABLES `cliente` WRITE;
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` VALUES (1,'Matias Baiss','2474492062','Mar del Plata 665','no'),(2,'Matias Bais','2474492062','Mar del Plata 665','no'),(3,'Pamela Tononi','2474476745','Mar del Plata 665','habilitado'),(4,'Ignacio Bais','0303456','Mar del Plata 665','no'),(5,'Carmen Vetrano','','','habilitado'),(6,'Nora Colaprete','','','habilitado'),(7,'Maria Badia','','','habilitado'),(8,'Lorena Rossier','','','habilitado'),(9,'Paola Michaud','','','habilitado'),(10,'Pamela Sabatini','','','habilitado'),(11,'Adrian Zucaro','','','habilitado'),(12,'Jeronimo Roldan','','','habilitado'),(13,'jjuan','','','no'),(14,'asd',NULL,NULL,'no'),(15,'asdasd',NULL,NULL,'no'),(16,'juan',NULL,NULL,'no'),(17,'asdasdd',NULL,NULL,'no'),(18,'asdasd',NULL,NULL,'no'),(19,'adas',NULL,NULL,'no'),(20,'asd',NULL,NULL,'no'),(21,'Ignacio Bais',NULL,NULL,'habilitado');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gasto`
--

DROP TABLE IF EXISTS `gasto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gasto` (
  `idgasto` int NOT NULL AUTO_INCREMENT,
  `Descripcion` varchar(45) DEFAULT NULL,
  `Valor` double DEFAULT NULL,
  `fecha` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idgasto`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gasto`
--

LOCK TABLES `gasto` WRITE;
/*!40000 ALTER TABLE `gasto` DISABLE KEYS */;
/*!40000 ALTER TABLE `gasto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedido`
--

DROP TABLE IF EXISTS `pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedido` (
  `idPedido` int NOT NULL AUTO_INCREMENT,
  `Fecha` datetime DEFAULT CURRENT_TIMESTAMP,
  `Pago` tinyint(1) DEFAULT '0',
  `idCliente` int NOT NULL,
  `estado` varchar(45) DEFAULT 'habilitado',
  `entregado` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`idPedido`),
  KEY `fk_Pedido_Cliente1_idx` (`idCliente`),
  CONSTRAINT `fk_Pedido_Cliente1` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (25,'2022-11-28 13:26:41',1,2,'habilitado',1),(26,'2022-11-28 13:27:07',0,3,'habilitado',0),(27,'2022-11-28 13:27:28',0,4,'habilitado',1),(28,'2022-11-28 19:19:41',0,5,'habilitado',0),(29,'2022-11-28 19:19:59',0,6,'habilitado',0),(30,'2022-11-28 19:20:22',0,7,'habilitado',0),(31,'2022-11-28 19:20:53',0,9,'habilitado',0),(32,'2022-11-28 19:21:19',0,10,'habilitado',0),(33,'2022-11-28 19:21:48',0,8,'habilitado',1),(34,'2022-11-28 19:22:20',0,8,'habilitado',0),(35,'2022-11-28 19:22:56',0,11,'habilitado',0),(36,'2022-11-28 19:23:15',0,12,'habilitado',0),(37,'2022-11-28 19:29:21',0,3,'habilitado',0),(38,'2022-11-28 19:37:45',0,3,'habilitado',0),(39,'2022-11-28 19:38:04',0,3,'habilitado',0),(40,'2022-11-28 19:47:11',0,3,'habilitado',0),(41,'2022-11-28 20:15:23',0,3,'habilitado',0),(42,'2022-11-27 00:00:00',0,3,'habilitado',0),(43,'2022-11-27 00:00:00',0,15,'habilitado',0),(44,'2022-11-27 00:00:00',0,18,'habilitado',0),(45,'2022-11-27 00:00:00',0,20,'habilitado',0),(46,'2022-12-04 00:00:00',0,3,'habilitado',0),(47,'2022-12-04 00:00:00',0,21,'habilitado',0),(48,'2022-11-27 00:00:00',0,3,'habilitado',0);
/*!40000 ALTER TABLE `pedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedidorenglon`
--

DROP TABLE IF EXISTS `pedidorenglon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedidorenglon` (
  `idpedidoRenglon` int NOT NULL AUTO_INCREMENT,
  `cantidad` varchar(45) DEFAULT NULL,
  `idPedido` int NOT NULL,
  `idProducto` int NOT NULL,
  PRIMARY KEY (`idpedidoRenglon`),
  KEY `fk_pedidoRenglon_Pedido1_idx` (`idPedido`),
  KEY `fk_pedidoRenglon_Producto1_idx` (`idProducto`),
  CONSTRAINT `fk_pedidoRenglon_Pedido1` FOREIGN KEY (`idPedido`) REFERENCES `pedido` (`idPedido`),
  CONSTRAINT `fk_pedidoRenglon_Producto1` FOREIGN KEY (`idProducto`) REFERENCES `producto` (`idProducto`)
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedidorenglon`
--

LOCK TABLES `pedidorenglon` WRITE;
/*!40000 ALTER TABLE `pedidorenglon` DISABLE KEYS */;
INSERT INTO `pedidorenglon` VALUES (21,'1',26,10),(22,'2',26,7),(24,'1',26,11),(25,'1',28,6),(26,'1',28,10),(27,'1',29,7),(28,'1',29,8),(29,'1',30,6),(30,'1',30,10),(31,'1',30,12),(32,'1',30,14),(33,'1',31,11),(34,'1',31,17),(35,'1',31,13),(36,'1',32,12),(37,'1',33,9),(38,'1',33,10),(39,'2',34,10),(40,'1',34,6),(41,'1',34,17),(42,'1',34,13),(43,'3',35,10),(44,'1',36,7),(45,'1',36,11),(46,'1',36,13);
/*!40000 ALTER TABLE `pedidorenglon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `precio`
--

DROP TABLE IF EXISTS `precio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `precio` (
  `idprecio` int NOT NULL AUTO_INCREMENT,
  `fechaDesde` datetime DEFAULT CURRENT_TIMESTAMP,
  `valor` double DEFAULT NULL,
  `idProducto` int NOT NULL,
  PRIMARY KEY (`idprecio`),
  KEY `fk_precio_Producto1_idx` (`idProducto`),
  CONSTRAINT `fk_precio_Producto1` FOREIGN KEY (`idProducto`) REFERENCES `producto` (`idProducto`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `precio`
--

LOCK TABLES `precio` WRITE;
/*!40000 ALTER TABLE `precio` DISABLE KEYS */;
INSERT INTO `precio` VALUES (1,'2022-11-27 23:31:46',600,1),(7,'2022-11-27 23:43:19',600,1),(10,'2022-11-27 23:50:48',600,1),(11,'2022-11-27 23:51:04',1100,5),(12,'2022-11-27 23:55:09',1100,6),(13,'2022-11-28 12:08:51',600,7),(14,'2022-11-28 12:08:58',600,8),(15,'2022-11-28 12:09:10',600,9),(16,'2022-11-28 12:09:17',1100,10),(17,'2022-11-28 12:09:23',600,11),(18,'2022-11-28 12:09:33',1100,12),(19,'2022-11-28 12:11:24',600,13),(20,'2022-11-28 12:11:30',1100,14),(21,'2022-11-28 12:11:37',600,15),(22,'2022-11-28 12:11:42',1100,16),(23,'2022-11-28 12:12:07',600,17),(24,'2022-11-28 12:12:12',1100,18);
/*!40000 ALTER TABLE `precio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producto`
--

DROP TABLE IF EXISTS `producto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producto` (
  `idProducto` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) DEFAULT NULL,
  `Unidad` varchar(45) DEFAULT NULL,
  `idCategoria` int NOT NULL,
  `estado` varchar(45) DEFAULT 'habilitado',
  PRIMARY KEY (`idProducto`),
  KEY `fk_Producto_Categoria_idx` (`idCategoria`),
  CONSTRAINT `fk_Producto_Categoria` FOREIGN KEY (`idCategoria`) REFERENCES `categoria` (`idCategoria`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producto`
--

LOCK TABLES `producto` WRITE;
/*!40000 ALTER TABLE `producto` DISABLE KEYS */;
INSERT INTO `producto` VALUES (1,'de Carne','6',1,'habilitado'),(2,'de Carne','12',1,'habilitado'),(3,'de Carne','12',1,'habilitado'),(5,'de Carne','12',1,'no'),(6,'de Carne','12',1,'habilitado'),(7,'de Carne','1',3,'habilitado'),(8,'de Verdura','1',3,'habilitado'),(9,'de Jamon y Queso','6',1,'habilitado'),(10,'de Jamon y Queso','12',1,'habilitado'),(11,'Arabes','6',1,'habilitado'),(12,'Arabes','12',1,'habilitado'),(13,'de Verdura','6',1,'habilitado'),(14,'de Verdura','12',1,'habilitado'),(15,'de Pollo','6',1,'habilitado'),(16,'de Pollo','12',1,'habilitado'),(17,'de Humita','6',1,'habilitado'),(18,'de Humita','12',1,'habilitado');
/*!40000 ALTER TABLE `producto` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed
