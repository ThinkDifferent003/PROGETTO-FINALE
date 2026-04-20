#scene: cella_buia
...
*[Gaurdati intorno]
L'odore è soffocante.
-> IncontroPrigioniero

===IncontroPrigioniero===
#scene: corridoio_celle
Dalla cella accanto, una voce familiare ti chiama, è morente.
"Aiutami...per favore...loro..."
-> LeGuardie

===LeGuardie===
#scene: ombre_nascondiglio
Senti passi pesanti in corridoio.
"Ci siamo proprio divertiti con quella, ahahah."
"Ora andiamo a torturare l'altro prigioniero."

[Combattimento]
Dopo averli sconfitti.
-> IlRitrovamento

===IlRitrovamento===
#scene: ufficio_guardie
Dopo aver attraversato stanze vuote , inciampi in qualcosa.
"Ti ho lasciato...ma era per poterti lasciare un futuro...fratellino , perdonami."
-> Finale

===Finale===
#scene: uscita_castello
Sei uscito, ma non sei solo.
"EHEH, sei uscito. Come sta la lingua?"
*[Stingi i pugni]
*[Silenzio]
-"Ci vediamo presto."

#event : sfuma_al_nero
L'oscurità ti avvolge di nuovo.
-> END