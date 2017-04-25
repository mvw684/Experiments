//by Sanne van Wijk and Julia Claassen
import java.util.*;
import java.io.FileWriter;
import java.io.File;
import java.io.IOException;
import java.io.FileNotFoundException;

public class Supremum{
  Scanner sc = new Scanner(System.in);
  int[] createsdouble = new int[256];
  String[] createsfewdouble = new String[256];
  int[] numbers = new int[256];
  File file = new File("test12.txt");
  //put message on console, yay
  void main(){
    int index = 0;
    int a = 1;
    while (index < 256){
        if (a < 20 || a % 6 == 0 || a % 10 == 0 || a % 15 == 0
            || a % 9 == 0 || a % 13 == 0
            || a % 21 == 0 || a % 23 == 0 || a % 32 == 0 || a % 33 == 0 
            || a % 31 == 0 || a % 28 == 0 
            || a % 34 == 0 || a % 35 == 0 || a % 41 == 0 || a % 53 == 0
            || a % 37 == 0 || a % 44 == 0|| a % 29 == 0
            || a % 38 == 0 || a % 43 == 0 || a % 55 == 0 || a % 57 == 0
            || a % 44 == 0 || a % 47 == 0 || a % 49 == 0
            || a % 51 == 0 ){
            numbers[index] = a;
            index++;
        }
        a++;
    }
      try{
          Scanner sc = new Scanner(file);
          for (int i = 0; i < 256; i ++){
              numbers[i] = sc.nextInt();
          }    
      } catch (FileNotFoundException e){
          e.printStackTrace();
      }
      Arrays.sort(numbers);
      System.out.println("Amount of Solutions is " + calculateSolutions(numbers));
      System.out.println(Arrays.toString(numbers));
      //System.out.println(Arrays.toString(createsdouble));
      writefile(numbers);
      improve();
  }
  
  void InsertionSort(int[] a){
      int i, j, temp;
      for (i = 1; i < a.length; i++)
      {
          j = i - 1;
          while (j >= 0 && a[j] > a[i] )
          {
              temp = a[i];
              a[i] = a[j];
              a[j] = temp;
              i=j;
              j--;
              
          }
      }
  }

  void improve(){
      int currentsolution = calculateSolutions(numbers);
      int best = currentsolution;
      System.out.println("starting the while loop now");
      //for (int k = 0; k < 100; k++){
      Boolean nothingreplaced = false;
      int count = 0;
      while (true){
          count++;
          /*
          replacewith++;
          if (replacewith > numbers[255] + numbers[254]){
              replacewith = 1;
              //there is no better solution
              if (nothingreplaced){
                  break;
              }
              //so far nothing has been replaced
              nothingreplaced = true;
          }*/
          //if (replacewith %100 == 0){System.out.println("Going to replacewith "+replacewith);}
          int replacewith0 = (int) (Math.random() * 2000 + 1);   
          //int replacewith1 = (int) (Math.random() * 2000 + 1);  
          //int replacewith2 = (int) (Math.random() * 2000 + 1);  
          Boolean nodoubles = true;
          for ( int i = 0; i < 256 ; i++){
              if (numbers[i] == replacewith0 ){//|| numbers[i] == replacewith1 || numbers[i] ==  replacewith2
                  nodoubles = false;
                 // System.out.println(i + " " + replacewith + " " + numbers[i]);
              }
          }
          //calculate the currentsolution with new value  
          if (nodoubles){
             // System.out.println("at least theres no doubles");
             // Boolean ofcourseonlyonce = true;
             // for( int index = 0; index < 256; index++){
                  int index0 = (int) (Math.random() * 256);
                 // int index1 = (int) (Math.random() * 256);
                //  int index2 = (int) (Math.random() * 256);
                  int remember0 = numbers[index0];
              //   int remember1 = numbers[index1];
               //   int remember2 = numbers[index2];
                  numbers[index0] = replacewith0;
               //   numbers[index1] = replacewith1;
              //    numbers[index2] = replacewith2;
                  currentsolution = calculateSolutions(numbers);
                  if (count % 10000 == 0){System.out.println(currentsolution);}
                  if (currentsolution < best){// && ofcourseonlyonce
                      //ofcourseonlyonce = false;
                      //nothingreplaced = false;
                      Arrays.sort(numbers);
                      System.out.println("Found An Improvement!");
                      System.out.println("added " + replacewith0);// + " "+ replacewith1 + " "+ replacewith2);
                      System.out.println("Amount of Solutions is " + currentsolution);
                      best = currentsolution;
                      System.out.println(Arrays.toString(numbers));
                      writefile(numbers); 
                  } else{
                      numbers[index0] = remember0;
                      //numbers[index1] = remember1;
                      //numbers[index2] = remember2;
                  }
          }
      }/*
      System.out.println("##############################");
      System.out.println("#ALL IS LOST NOTHING REPLACED#");
      System.out.println("##############################");
      System.out.println("############"+ best+ "#############");
      System.out.println("##############################");*/
  }

  Integer calculateSolutions(int[] array){
      Set<Integer> solutions = new HashSet<Integer>();
      int m;
      int a;
      int s;
      for (int i = 0; i < 256; i++){
        for (int k = 0; k < 256; k++){
            m = array[i]*array[k];
            a = array[i]+array[k];
            s = Math.abs(array[k] - array[i]);
            /*
            if ( solutions.contains(m)){
                createsdouble[i] += 1;
                createsdouble[k] += 1;
            }
            if (solutions.contains(a)){
                createsdouble[i] += 1;
                createsdouble[k] += 1;
            }
            if ( solutions.contains(s)){
                createsdouble[i] += 1;
                createsdouble[k] += 1;
            }
            */
            solutions.add(m);
            solutions.add(a);
            solutions.add(s);
        }
      }
      /*
      for (int i = 0; i < 256; i++){
        if (createsdouble[i] < 1425){
            createsfewdouble[i] = 1425 -createsdouble[i] + "---" + numbers[i];
        }
    }*/
      return solutions.size();
  }
  
  void writefile(int[] numbers){
      String formattedString = Arrays.toString(numbers)
          .replace(",", "")  //remove the commas
          .replace("[", "")  //remove the right bracket
          .replace("]", "")  //remove the left bracket
          .trim();
      try{
          FileWriter writer = new FileWriter(file);
          writer.write((formattedString));
          writer.close();
      } catch(IOException ex){
          ex.printStackTrace();
      }
  }
  
  
  //start-up code
  public static void main(String[] args){
    (new Supremum()).main();
  }
}
