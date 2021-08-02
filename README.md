Ukol 2

- Console app ... Lze spustit v adresari 'Top' (zadani), nebo cestu k takovemu adresari predat jako prvni parametr aplikace

- Aplikace najde vsechny *.xml soubory v podadresarich. U tech, ktere maji root element s nazvem "root" se snazi najit elementy "produkt" a vracet obsah elementu EAN. Pro demo jej jen vypise na consoli.

- V zadani neni uvedena predpokladana velikost datovych souboru, tedy je blbuvzdorne pouzit XmlReader - lze tak parsovat neomezene velke xml soubory, bez naroku na pamet. V pripade, ze by bylo znamo, ze soubory nepresahnou jednotky MB, lze pouzit System.Xml.Linq, nicmene zapis nebude o moc kratsi ;)

- Odchylka od zadani je v tom, ze neresi poznamku "Xml soubory jsou na stejne urovni" - tzn. vezme v potaz vsechny xml soubory v predane adresarove strukture



