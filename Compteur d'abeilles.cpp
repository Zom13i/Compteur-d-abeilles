//Sensors array
int sensorGateA[19]; //Alfa sensor array
int sensorGateB[19]; //Beta sensor array

//Sensor array states
int gateALastStat[19];  //Alfa sensor state
int gateBLastStat[19];  //Beta sensor state
int gateSensPassag[19]; //passage sens in the gate [0] = nothing, [1] = in, [2] = out.

//Time variables
unsigned long currentTime = 0;      //run time of Mega
unsigned long gateStartPassage[19]; //start passage time per gate
unsigned long gateEndPassage[19];   //end passage time per gate
unsigned long delayGate = 0;        //common passage time
unsigned long sumDelayGate = 0;     //sum of all delays

//incremental variables
int ins = 0;            //counts ins and outs
int outs = 0;           //qty outputs
int quantity = 0;       //Inside the hive
int i = 0;              //lambda incremental
int nbGatePassages = 0; //Qty of passages per 20 sensors
int bootReady = 0;

int count = 0; // this just tests if there has been a change in our bee count
int lcount = 0;

void setup()
{
  // initialize sensors as an input + set delay to 0:
  // le compte commence à partir de 13. DealayGate de 0 à 19.
  for (i = 13; i < 15; i++)
  {
    pinMode(i, INPUT);
    pinMode((i + 20), INPUT);

    gateALastStat[i - 13] = 0;
    gateBLastStat[i - 13] = 0;
    gateStartPassage[i - 13] = 0;
    gateEndPassage[i - 13] = 0;
    gateSensPassag[i - 19] = 0;
  }

  //pine de test c'est ma teub
  //pinMode(29, OUTPUT);
  // pinMode(bin, INPUT);
  // pinMode(bout, INPUT);

  // initialize serial communication:
  Serial.begin(38400); //a bit different than the Arduino here.... 38400
}

void loop()
{
  //Boot delay
  if (bootReady != 0)
  {
    delay(1000);
    bootReady = 1;
  }

  //Read Arduino time
  currentTime = millis();

  // read all sensors:
  //commencer compter à partir de
  for (i = 13; i < 15; i++)
  {
    //lecture des capteurs
    sensorGateA[i - 13] = digitalRead(i);
    sensorGateB[i - 13] = digitalRead(i + 20);

    //reset gate memorys if gate Ax and Bx are both down ( need to avoid +1-1 bug)
    if ((sensorGateA[i - 13] == 0) and (sensorGateB[i - 13] == 0))
    {
      gateALastStat[i - 13] = 0;
      gateBLastStat[i - 13] = 0;
      gateStartPassage[i - 13] = 0;
      gateEndPassage[i - 13] = 0;
      gateSensPassag[i - 13] = 0;
    }

    // if gate A  = 1 so memory gate = 1. Also read time
    if ((sensorGateA[i - 13] == 1) and (gateALastStat[i - 13] == 0))
    {
      gateALastStat[i - 13] = 1;
      gateSensPassag[i - 13] = 1;
      gateStartPassage[i - 13] = millis();
    }

    // if gate B  = 1 so memory gate = 1. Also read time
    if ((sensorGateB[i - 13] == 1) and (gateBLastStat[i - 13] == 0))
    {
      gateBLastStat[i - 13] = 1;
      gateSensPassag[i - 13] = 2;
      gateStartPassage[i - 13] = millis();
    }
  }

  //Compare input
  for (i = 0; i < 2; i++)
  {

    //=======================================================
    //Calculate what was the dirrection of passage
    if ((gateALastStat[i] == 1) and (gateBLastStat[i] == 1))
    { //If gate memories are in the transition state

      //2 bees in Gate collision (  -->[]  []<--  )
      if (((gateEndPassage[i] - gateStartPassage[i]) < delayGate) and (nbGatePassages > 3))
      {
        //Reset gate memory
        gateALastStat[i] = 0;
        gateBLastStat[i] = 0;

        continue;
      }

      //If the gate A is 0 => understand it as exit
      if ((sensorGateA[i] == 0) and (sensorGateB[i] == 1))
      {

        //read exiting time
        gateEndPassage[i] = millis();

        //delai use
        if ((((gateEndPassage[i]) - (gateStartPassage[i])) > (delayGate * 4)) and delayGate != 0)
        {
          ins = ins + (((gateEndPassage[i] - gateStartPassage[i]) / delayGate) - ((gateStartPassage[i] - gateStartPassage[i]) % delayGate));
        }
        else
        {
          ins++; //incrementer

          //
          nbGatePassages++;
          sumDelayGate = sumDelayGate + (gateEndPassage[i] - gateStartPassage[i]);
        }

        //Reset gate memory
        gateALastStat[i] = 0;
        gateBLastStat[i] = 0;
      }

      //si gate B est à 0 => considerer comme sortie
      if ((sensorGateB[i] == 0) and (sensorGateA[i] == 1))
      {

        outs++; //décrementer

        gateEndPassage[i] = millis(); //lire moment de passage fin

        //delai use
        if ((((gateEndPassage[i]) - (gateStartPassage[i])) > (delayGate * 4)) and delayGate != 0)
        {
          outs = outs + (((gateEndPassage[i] - gateStartPassage[i]) / delayGate) - ((gateStartPassage[i] - gateStartPassage[i]) % delayGate));
        }
        else
        {
          outs++; //décrementer
        }

        sumDelayGate = sumDelayGate + (gateEndPassage[i] - gateStartPassage[i]);
        nbGatePassages++;

        //Reset gate memory
        gateALastStat[i] = 0;
        gateBLastStat[i] = 0;
      }
      //=======================================================

      //calculer la moyenne de passage
      delayGate = (sumDelayGate / nbGatePassages);
    }
    //reset vars
    //sumDelayGate = 0;
    //nbGatePassages = 0;
  }

  //compter les abeilles dans la ruche
  if (outs <= ins)
  {
    count = ins - outs;
  }
  else
  {
    ins = outs;
  }

  if ((lcount != count) or ((currentTime / (currentTime / 1000)) == 1000))
  { // if the count has changed we print the new count

    Serial.println("=============================================================");
    Serial.println("---IN------OUT-----INSIDE---");
    Serial.print("--");
    Serial.print(ins);
    Serial.print("--");
    Serial.print(outs);
    Serial.print("--");
    Serial.print(count);
    Serial.println("--");
    Serial.print("delai moyen = ");
    Serial.println(delayGate);
    Serial.println("=============================================================");

    //serial write test
    Serial.println(sensorGateA[0]);
    Serial.println(gateALastStat[0]);
    Serial.println(sensorGateB[0]);
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

/// BLABLALBLABLALBALBALBLA