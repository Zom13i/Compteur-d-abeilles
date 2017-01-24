

//Sensors array (physic state)
int sensorRangeA [20];                            //Alfa sensor array 
int sensorRangeB [20];                            //Beta sensor array

//Sensor array states (memoy state)
int gateALastStat [20];                          //Alfa sensor state
int gateBLastStat [20];                          //Beta sensor state
//int gateSensPassag[20];                          //passage sens in the gate [0] = nothing, [1] = in, [2] = out.

//Time variables
unsigned long currentTime = 0;                    //run time of Mega
unsigned long gateStartPassage[20];               //start passage time per gate
unsigned long gateEndPassage[20];                 //end passage time per gate
unsigned long delayGate = 0;                      //common passage time
unsigned long sumDelayGate = 0;                 //sum of all delays

//incremental variables
int ins = 0;                               //counts ins and outs
int outs = 0;                              //qty outputs    
int quantity = 0;                          //Inside the hive
int i = 0;                                //lambda incremental 
int nbGatePassages = 0;                   //Qty of passages per 20 sensors


int bootReady = 0;


int count = 0;                                   // this just tests if there has been a change in our bee count
int lcount = 0;
int difference = 0;

SoftwareSerial mavoieserie(9, 8);

void setup() {                                   
  // initialize sensors as an input + set delay to 0:
  // le compte commence à partir de 13. DealayGate de 0 à 19.
  for (i = 2; i < 4; i++){
    pinMode(i, INPUT);
    pinMode((i + 2), INPUT);
    
  gateALastStat[i - 2] = 0;
  gateBLastStat[i - 2] = 0;
  gateStartPassage[i-2] = 0;
  gateEndPassage[i-2] = 0;
  //gateSensPassag[i-19] = 0;
  }


  // initialize serial communication:
  Serial.begin(38400);                     //a bit different than the Arduino here.... 38400
 
}


void loop() {
  //Boot delay (startup sequence)
  if (bootReady != 0 ){
    delay(1000);
    bootReady = 1;
  }

  //Read Arduino time
  currentTime = millis();
  
  if (mavoieserie.available()) {
        Serial.write(mavoieserie.read());
    }
    if (Serial.available()) {
        mavoieserie.write(Serial.read());
    }
  //==============================================================================================================
  // read all sensors:
  //Start from physic pin 13 
  for (i = 2; i < 4; i ++) {
    //lecture des capteurs
    sensorRangeA[i-2] = digitalRead(i);
    sensorRangeB[i-2] = digitalRead(i + 2);


      
    //reset gate memorys if gate Ax and Bx are both down ( need to avoid +1-1 bug)
    if (( sensorRangeA[i-2] == 0) and ( sensorRangeB[i-2] == 0)) {
      gateALastStat[i - 2] = 0;
      gateBLastStat[i - 2] = 0;
      gateStartPassage[i-2] = 0;
      gateEndPassage[i- 2] = 0;
      //gateSensPassag[i - 13] = 0;
     }

    // if gate A  = 1 so gate memory  = 1. Also read time   ( -->[A]__[B]__  )
    if (( sensorRangeA[i-2] == 1) and (gateALastStat[i-2] != 2)) {

      gateALastStat[i-2] = 1;
      //gateSensPassag[i - 13] = 1;
    gateStartPassage[i-2] = millis(); 
      //gateBLastStat[i-13] = 0;     
    }
    else if ((sensorRangeA[i-2] == 0) and (gateALastStat[i-2] == 2) ){
      gateALastStat[i-2] = 0;
      }
      
    
   
    // if gate B  = 1 so memory gate = 1. Also read time   ( __[A]__[B]<--  )
    if (( sensorRangeB[i-2] == 1) and (gateBLastStat[i-2] != 2)){
      gateBLastStat[i-2] = 1;
      //gateSensPassag[i - 13] = 2;
    gateStartPassage[i-2] = millis();
     //gateALastStat[i-13] = 0;
    }
    else if ((sensorRangeB[i-2] == 0)and (gateBLastStat[i-2] == 2)  ){
      gateBLastStat[i-2] = 0;
    }

   
  }
  
  
  

  //Compare input for every gate
  for (i = 0; i < 2; i++){

  //==============================================================================================================
  //Calculate what was the dirrection of passage
  if ((gateALastStat[i] == 1) and ( gateBLastStat[i] == 1)){            //If gate memories are in the transition state

    //2 bees in Gate collision (  -->[]  []<--  )   
    if (( (gateEndPassage[i] - gateStartPassage[i]) < delayGate ) and (nbGatePassages > 3)){
      //Reset gate memory
      gateALastStat[i] = 0;
      //gateBLastStat[i] = 0;

      continue;
    }
    

    //If the gate A is 0 => understand it as exit ( --[A]--[B]--> )
    if  ((sensorRangeA[i] == 0) and (sensorRangeB[i] == 1)){
      
      //read exiting time
      gateEndPassage[i] = millis();  


        ins++;    //incrementer 
        //
        nbGatePassages++;
        sumDelayGate = sumDelayGate + (gateEndPassage[i] - gateStartPassage[i]);
      

      //Reset gate memory
      gateALastStat[i] = 0;
      gateBLastStat[i] = 2;
    }
   
    //si gate B est à 0 => considerer comme sortie ( <--[A]--[B]-- )
    if  ((sensorRangeB[i] == 0) and (sensorRangeA[i] == 1)){
       
      
      gateEndPassage[i] = millis();   //lire moment de passage fin

      

    outs++;    //décrementer  
      

      
      sumDelayGate = sumDelayGate + (gateEndPassage[i] - gateStartPassage[i]);
      nbGatePassages++;

      //Reset gate memory
    gateALastStat[i] = 2;
    gateBLastStat[i] = 0;
    }
    
    //==============================================================================================================

    
    //calculer la moyenne de passage
    delayGate = (sumDelayGate/nbGatePassages);
    
  
  }
  
  }
   

  
  //compter les abeilles dans la ruche
  if (outs <= ins) {
  count = ins - outs;
  }
 
  
  if ((lcount != count) or ((currentTime / (currentTime/1000)) == 1000  ))  {          // if the count has changed we print the new count
  
  Serial.println("=============================================================");
    Serial.println("---IN------OUT-----INSIDE---");
      Serial.print("--");
        Serial.print(ins);
      Serial.print("--");
        Serial.print(outs);
      Serial.print("--");
        Serial.print(count);
      Serial.println("--");  
    Serial.print("delai moyen = " );
      Serial.println(delayGate);
    Serial.println("=============================================================");

    //serial write test
    Serial.println(sensorRangeA[0]);
    Serial.println(gateALastStat[0]);
    Serial.println(sensorRangeB[0]);
    Serial.println(gateBLastStat[0]);
    Serial.println(currentTime);
    Serial.println(gateStartPassage[0]);
    Serial.println(gateEndPassage[0]);
    Serial.println(sumDelayGate);
    Serial.println(nbGatePassages);
  Serial.println("\n");


    lcount = count;
  }
   
   
  
}