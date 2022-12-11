import { Component, ElementRef, OnChanges, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/interfaces/user';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnChanges, OnDestroy {

  public text:string = "";

  public isDisabled:boolean = false;

  public user: User = {
    id : 0,
    username : '',
    email : '',
    firstName :'',
    lastName : '',
    password: '',
    location: '',
    walletAddress:'',
    aboutMe : '',
    address:'',
    phone:'',
    title:'',
    role: -1,
    birthday: new Date(),
    iv:null,
    amount:null
  }

  public email:string = '';
  public password:string = '';

  public error:boolean | string = false;

  @ViewChild("countdown")
  myDiv!: ElementRef;

  @ViewChild("loginButton") loginButton!:any;

  private IvEnv:CryptoJS.lib.WordArray  = CryptoJS.lib.WordArray.random(16);
  private keyEnv: string = environment.key;
  
  validateEmail(email:string) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
  }
  constructor(private authService:AuthService, private router:Router) { }

  ngOnInit(): void {
    var token = localStorage.getItem('token');
    if(token != undefined && token != null){
      let id = this.parseJwt(token).id
      if( id != null && id != '' ){
        this.router.navigate(['/home']).then(() => {
          window.location.reload();
        });
      }
    }

  }


  parseJwt(token:string|null|undefined) {
    if(token != null && token != undefined){
      var base64Url = token.split('.')[1];
      var base64 = base64Url.replace('-', '+').replace('_', '/');
      return JSON.parse(window.atob(base64));
    }

  }

  doLogin():void{
    this.error = false;

    if(this.validateEmail(this.email)){
      let data = {
        "email":this.authService.set(this.email,this.keyEnv,this.IvEnv),
        "password":this.authService.set(this.password,this.keyEnv,this.IvEnv),
        "iv":this.authService.wordArrayToByteArray(this.IvEnv,16)
      }
      try{

        const observer = {
          next: (response: any) => 
            {
              localStorage.setItem('token',response.data.token);
              localStorage.setItem('name',response.data.lastName + ' ' + response.data.firstName)
              this.router.navigate(['/home']).then(() => {
                window.location.reload();
              });
            },
          error: (err:any) =>{
              this.loginButton.nativeElement.disabled = true;
              //Afiseaza fereastra de succes

              const message = document.getElementById('failedMessage');
              if(message != null){
                message.innerHTML = err.error.message;
              }
              const element = document.getElementById('failedPopup');
              if(element != null){
                element.style.opacity = "100%";
                element.style.zIndex = "10";
                element.style.marginTop = "20%"
                setTimeout( () => {
                  element.style.visibility = "0%";
                  element.style.zIndex = "-1";
                  element.style.marginTop = "5%"
                }, 3000);
              }
              var timeleft = 10;
              var downloadTimer = setInterval(() =>{
                  if(timeleft <= 0){
                    clearInterval(downloadTimer);
                    this.myDiv.nativeElement.innerHTML = null;
                    this.loginButton.nativeElement.disabled = false;
                  } else {
                    this.myDiv.nativeElement.innerHTML = "(" +timeleft +")";
                  }
                  timeleft -= 1;
              }, 1000);
        },
          complete: () => console.log('Observer got a complete notification'),
        }
        this.authService.login(data).subscribe(observer);
      }
      catch(e){
        console.log("Exceptia :")
        console.log(e);
      }

    }else{
      this.error = "Email is not valid!";
    }

  }

  ngOnChanges(){

  }

  ngOnDestroy(){

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