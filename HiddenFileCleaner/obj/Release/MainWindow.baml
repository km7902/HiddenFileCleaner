   M S B A M L   `   `   `  ����   HiddenFileCleaner"    HiddenFileCleaner.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  HiddenFileCleaner��$	��800��$	��450��$	��480��$	��320�� � 
ResizeMode$ CanResizeWithGrip=� � Icon$ HiddenFileCleaner.ico��.��]� .+�o� 
������ $	��Top=�.@�m� $��ファイル(_F)��.@�m� -   $��
終了(_X)��m� $��オプション(_O)��.@�m� -    m�Name$ MenuOption0�� m� IsCheckable .  m� 	IsChecked . $ ��.DS__Store を検索する��m� -   $ MenuOption1�� .  . $��.__* を検索する��m� -   $ MenuOption2�� .  . $��.apdisk を検索する��m� -   $ MenuOption3�� .  . $��Thumbs.db を検索する��m� -   $ MenuOption4�� .  . $!��Desktop.ini を検索する��m� -   $��バージョン情報(_A)���� $��Bottom=�.@��� ���� *Status.StatusText+� $��
10,0,10,10q� � RowDefinitions �� �� $
��Auto � � ColumnDefinitions �� $
��Auto ��� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��5��$
��0,10q�
����������� *List+ � 	Resources 	 �� 
TargetType
 �� Property �� Value   tpl_show d� 	IsChecked��%Z   '��       &	  %     �� "	 ��.��� 
 � ��$
 Left=��� .R���  �� *Select+ �� View �� .P��� $��選択�� �� CellTemplate# [   �� Width$ 40���� $��ファイル名��$	 100�� �� DisplayMemberBinding �� *
FileName+�� $��フォルダーパス��$	 320�� �� *
FolderName+�� $��作成日時��$	 120�� �� *FileCreateDate+�� $��更新日時��$	 120�� �� *FileModifyDate+�� $��サイズ (Byte)��$ 80�� �� *
FileSize+�� $��フォルダー��$��1��$��0��$��0,0,10,0q��� -    ��Name$ TextFolderPath��$��1��$��1�� �� TextWrapping$ NoWrap=�$��Center=����� *Data.FolderPath+.���� -	    ��Name$ BtnSelectFolder��$	��...��$��1��$��2��$��0,0,10,0q�$��5q��� -
   $ BtnSearchFolder��$��検索��$��1��$��3��$��0,0,10,0q�$
��24,0q��� -   $ BtnAbortSearchFolder��$��中止��$��1��$��3��$��0,0,10,0q�$
��24,0q�$y�Hidden=��� -   $ BtnDeleteFile��$��削除��$��1��$��4��$
��24,0q��� -   $ BtnAbortDeleteFile��$��中止��$��1��$��4��$
��24,0q�$y�Hidden=�