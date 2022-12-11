import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/interfaces/user';
import * as CryptoJS from 'crypto-js';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {


  private IvEnv:CryptoJS.lib.WordArray  = CryptoJS.lib.WordArray.random(16);
  private keyEnv: string = environment.key;
  public myForm!:UntypedFormGroup;
  @ViewChild("countdown")
  myDiv!: ElementRef;

  @ViewChild("registerButton") registerButton!:any;
  
  private user: User = {
    id : null,
    username : null,
    email : null,
    firstName : null,
    lastName : null,
    password: null,
    location: null,
    walletAddress:null,
    aboutMe : null,
    address:null,
    phone:null,
    title:null,
    role: null,
    birthday: null,
    iv:null,
    amount:null
  }
  constructor(private formBuilder:UntypedFormBuilder, private authService:AuthService,private router:Router) { }

  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      email:['',[Validators.required,Validators.email]],
      firstName:['',[Validators.required]],
      lastName:['',Validators.required],
      password:['',[Validators.required,Validators.minLength(5)]],
      birthday :['',[Validators.required]],
      Iv :[]
    })
  }

  doRegister(){

    if(this.myForm.valid){
      console.log("Form trimis la validare")
      console.log(this.myForm);

      let email:string = this.authService.set(this.myForm.controls["email"].value,this.keyEnv,this.IvEnv);
      let firstName:string = this.authService.set(this.myForm.controls["firstName"].value,this.keyEnv,this.IvEnv);
      let lastName:string = this.authService.set(this.myForm.controls["lastName"].value,this.keyEnv,this.IvEnv);
      let password:string = this.authService.set(this.myForm.controls["password"].value,this.keyEnv,this.IvEnv);
      let birthday = this.myForm.controls["birthday"].value;
      var bytearray:number[]= this.authService.wordArrayToByteArray(this.IvEnv,16);

      this.user.email = email;
      this.user.firstName = firstName;
      this.user.lastName = lastName;
      this.user.password = password;
      this.user.birthday = birthday;
      this.user.iv = bytearray;
      this.user.amount = 0;


      const observer = {
        next: (response: any) => 
          {
            this.myForm.reset();
            this.router.navigate(['/login']);
          },
        error: (err:any) =>{
            this.registerButton.nativeElement.disabled = true;
            //Afiseaza fereastra de succes
            const message = document.getElementById('failedMessage');
            if(message != null){
              message.innerHTML = err.error.message;
            }
            const element = document.getElementById('failedPopup');
            if(element != null){
              element.style.display = "block";
              element.style.opacity = "100%";
              element.style.zIndex = "10";
              element.style.marginTop = "25%"
              setTimeout( () => {
                element.style.visibility = "0%";
                element.style.zIndex = "-1";
                element.style.marginTop = "5%"
                element.style.display = "none";
              }, 3000);

            }


            var timeleft = 15;
            var downloadTimer = setInterval(() =>{
                if(timeleft <= 0){
                  clearInterval(downloadTimer);
                  this.myDiv.nativeElement.innerHTML = null;
                  this.registerButton.nativeElement.disabled = false;
                } else {
                  this.myDiv.nativeElement.innerHTML = "(" +timeleft +")";
                }
                timeleft -= 1;
            }, 1000);
      },
        complete: () => console.log('Observer got a complete notification'),
      }


      this.authService.register(this.user).subscribe(observer)
    }
  }


  closePopup(){
    const element = document.getElementById('failedPopup');
        if(element != null){
          element.style.visibility = "hidden";
          element.style.visibility = "0%";
          element.style.zIndex = "-1";
          element.style.marginTop = "5%"
          element.style.visibility = "visible";
        }
  }
}