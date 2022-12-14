# DirectoryScanner

## Состав репозитория
	* .Net6.0 ConsoleApplication (WinExe)
	* .Net6.0 Библиотека классов

## Описание библиотеки классов
В библиотеке классов содержаться 3 класса модели:
* Item
* File : Item
* Folder : Item

и 2 класса обработчика:
* EnviromentProvider
* GenerateDocument

Класс EnviromentProvider рекурсивно собирает информацию о структуре рабочей дериктории и всех ее sub-директорий, эти данные структурируются в 2х структурах:
1. Экземпляр класса Folder, в котором содержится коллекция файлов (File) и sub-директорий (Folder), в которых, в свою очередь содержаться их дочерние объекты
2. Список экземпляров класса File, Которые не отображают структуры директории, однако проще поддаются статистическому анализу

Класс GenerateDocument принимает в конструкторе экземпляр класса System.Windows.Forms.HtmlDocument (создание экземпляра этого класса зависит от целевой платформы).
Далее к нему можно применить один или несколько методов:
1. ***AddTreeView(Folder enviroment)*** - Воссаздает в HTML документе структуру рабочей директории на основании данных, полученых из экземпляра класса Folder
2. ***AddMimeStatistics(List<File> files)*** - Добавляет на HTML документ таблицу, содержащую следующие данные:
	* Название MimeType
	* Как часто встречается данный MimeType относительно всей собранной коллекции(количественное и процентное отношение)
	* Сумаа размеров всех файлов каждого MimeType
	* средний размер файла для каждого MimeType
3. ***SaveDocument()*** - Сохраняет HtmlDocument как .html файл в рабочей директори

## Исползованные технологии:
* .Net6.0
* System.IO
* System.Windows.Forms
* MimeTypes2.4.0
* Costura.Fody5.7.0 (окончательная упаковка производилась средствами публикации VisualStudio 2022)
