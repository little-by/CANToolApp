   M S B A M L   `   `   `  ���� E  ACANToolApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:LedDigitalDemoLedDigitalDemo  E ALedDigital, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:LedDigital;assembly=LedDigital
LedDigital F BColorPicker, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:ColorPicker;assembly=ColorPickerColorPicker     CANToolApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:LedDigitalDemo     <led,clr-namespace:LedDigital;assembly=LedDigital     =cp.clr-namespace:ColorPicker;assembly=ColorPicker       � Title$  
MainWindow��$��227.411��$	��525��.��� .+���  �� Orientation$ Vertical=�.+��� ���� *LedDigitalValue+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� ���� *DigitalCount+$ TwoWay=�$ PropertyChanged=�$  LedDigital.DigitalPanelControl  -     Name$ ledTest��$	��500��$��60��$��	0,10,0,10q�   DigitalColor$
 Blue��   Value �� *LedDigitalValue+$ OneWay=�$ PropertyChanged=�   DigitalCount �� *DigitalCount+����  �� EndPoint$ 0.5,1&�	 �� 
StartPoint$	 0.5,0&�.��� 
 � Color$
 	#FFF1DDF1��� $
 	#FFE1C3E4�� � Offset$ 1Q�� $
 	#FFEABFEE��$ 0.816Q�� $
 	#FFF474F4��$ 0.441Q�  -   $ ledTest1��$	��500��$��60��$��	0,10,0,10q�$ 19��$	 Red�� �� *LedDigitalValue1+$ OneWay=�$ PropertyChanged=����� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFF1DDF1��� $
 	#FFE1C3E4��$ 1Q�� $
 	#FFEABFEE��$ 0.816Q�� $
 	#FFF474F4��$ 0.441Q�