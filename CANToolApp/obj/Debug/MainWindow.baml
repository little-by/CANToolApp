   M S B A M L   `   `   `  ����E  ACANToolApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:LedDigitalDemoLedDigitalDemo  5   	   E ALedDigital, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:LedDigital;assembly=LedDigital
LedDigital 5   	   F BColorPicker, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:ColorPicker;assembly=ColorPickerColorPicker 5   	       CANToolApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:LedDigitalDemo     5   	   <led,clr-namespace:LedDigital;assembly=LedDigital     5   	   =cp.clr-namespace:ColorPicker;assembly=ColorPicker     5   	     � Title$  
MainWindow��5
   	   $��227.411��6   $	��525��6-   .��5      � .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      �� ��6   �� *LedDigitalValue+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5      ��6   �� *DigitalCount+$ TwoWay=�$ PropertyChanged=�$  LedDigital.DigitalPanelControl  5      -   5          Name$ ledTest��5   &   $	��500��67   $��60��6C   $��	0,10,0,10q�6O      DigitalColor$
 Blue��5         Value 5      �� *LedDigitalValue+$ OneWay=�$ PropertyChanged=�   DigitalCount 5      �� *DigitalCount+��5      �� 5       �� EndPoint$ 0.5,1&�6*   	 �� 
StartPoint$	 0.5,0&�6;   .��5      � 
 � Color$
 	#FFF1DDF1��6'   � 5      $
 	#FFE1C3E4��6'    � Offset$ 1Q�69   � 5      $
 	#FFEABFEE��6'   $ 0.816Q�69   � 5      $
 	#FFF474F4��6'   $ 0.441Q�69   5      5        5      -   5        $ ledTest1��5   &   $	��500��68   $��60��6D   $��	0,10,0,10q�6P   $ 19��5       $	 Red��5!       5      �� *LedDigitalValue1+$ OneWay=�$ PropertyChanged=���5"      �� 5#      $ 0.5,1&�6*   $	 0.5,0&�6;   .��5$      � $
 	#FFF1DDF1��6'   � 5%      $
 	#FFE1C3E4��6'   $ 1Q�69   � 5&      $
 	#FFEABFEE��6'   $ 0.816Q�69   � 5'      $
 	#FFF474F4��6'   $ 0.441Q�69   5(      5*      5+      5,      5-      