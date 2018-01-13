Import-Module -Name Scripts\Initialize-PowerCLIEnvironment.ps1
Import-Module 'ActiveDirectory'

Class Connect
{
    [int] $param
    static [Connect] $instance
    static [Connect] GetInstance()
    {
        if ([Connect]::instance -eq $null)
        {
            [Connect]::instance = [Connect]::new()
        }

        return [Connect]::instance
    }

    Connection()
    {
        Connect-VIServer gklab-157-005
    }
}


Class VCenter
{
    [Array]ListTemplates()
    {
        $list = New-Object System.Collections.ArrayList
        $list = Get-ResourcePool -Name "Templates" | Get-VM | Sort-Object
        return $list 
    }

    [Array]WindowsListTemplates()
    {
        $list = New-Object System.Collections.ArrayList
        $list = Get-ResourcePool -Name "Templates" | Get-VM | Where{$_.Guest.OSFullName -match 'Windows'} | Sort-Object
        return $list 
    }
}

Class Credentials
{
    [PSCredential]CredRoot()
    {
        $KeyFile = "c:\passwordstore\AES.key"
        $key = Get-Content $KeyFile    
        $RootUser = "root"
        $RootPasswordFile = "c:\passwordstore\Root_Pass.txt"
        return $GC = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $RootUser, (Get-Content $RootPasswordFile | ConvertTo-SecureString -Key $key)
    }

    [PSCredential]CredAdmin()
    {
        $KeyFile = "c:\passwordstore\AES.key"
        $key = Get-Content $KeyFile
        $AdmUser = "administrator"
        $AdmPasswordFile = "c:\passwordstore\Adm_Pass.txt"
        return $Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdmUser, (Get-Content $AdmPasswordFile | ConvertTo-SecureString -Key $key)
    }

    [PSCredential]CredGK()
    {
        $KeyFile = "c:\passwordstore\AES.key"
        $key = Get-Content $KeyFile
        $GKBUser = "gkblditp"
        $GKBPasswordFile = "c:\passwordstore\GKB_Pass.txt"
        return $gkblditp_cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $GKBUser, (Get-Content $GKBPasswordFile | ConvertTo-SecureString -Key $key)       
    }
}

Class NetSet
{
    [string]EpsilonID()
    {
        $IP_list=@()
        $IP_list=Invoke-RestMethod -UseDefaultCredentials -Method GET -Uri 'https://epsilon-next.igk.intel.com/api/IPs/ListFree/10091078000'
        $id_ip = $IP_list[0].id
        #$id_ip= 10091078001  #testowa linia
        return $id_ip
    }
    [string]EpsilonMac($id_ip)
    {
        $v0='00:50:56:'
        $id_ip=$id_ip.ToString()
        $v1=$id_ip.substring(5,2)
        $v2=$id_ip.substring(7,2)
        $v3=$id_ip.Substring(9,2)
        $mac='{0}{1}:{2}:{3}' -f $v0,$v1,$v2,$v3
        return $mac
    }
    EpsilonRes($mac,$id_ip)
    {
        $dic =@{
        'mac'=$mac;
        "description" =  "reservation created "
        "next_server"=  $null;
        "filename"=  $null;
        "root_path"=  $null ;
        "is_depcom"=  $false
        } | ConvertTo-Json
        Invoke-RestMethod -UseDefaultCredentials -Method PUT -Uri "https://epsilon-next.igk.intel.com/api/Ips/Reserve/$id_ip" -Body $dic -ContentType 'application/json' | Out-Null      
    }
    [string]HostName([string]$id_ip) 
    {
        $g1=$id_ip.substring(0,2)
        $g2=$id_ip.substring(2,3)
        $g3=$id_ip.substring(5,3)
        $g4=$id_ip.substring(8,3)
        $g0='gklab'
        if ($g3.substring(0,1) -eq '0') {$g3 = $g3.substring(1,2)}
        $VMname='{0}-{1}-{2}' -f $g0,$g3,$g4
        return $VMname
    }

    [string]IpAddress([string]$id_ip) 
    {
        $g1=$id_ip.substring(0,2)
        $g2=$id_ip.substring(2,3)
        $g3=$id_ip.substring(5,3)
        $g4=$id_ip.substring(8,3)
        if ($g3.substring(0,1) -eq '0') {$g3 = $g3.substring(1,2)}
        if ($g2.substring(0,1) -eq '0') {$g2 = $g2.substring(1,2)}
        if ($g3.substring(0,1) -eq '0') {$g3 = $g3.substring(1,1)}
        if ($g4.substring(0,2) -eq '00') {$g4 = $g4.substring(2,1)}
        if ($g4.substring(0,1) -eq '0') {$g4 = $g4.substring(1,2)}
        $ip_addrn='{0}.{1}.{2}.{3}' -f $g1,$g2,$g3,$g4
        return $ip_addrn     
    }
    [string]SelectSAN()
    {
        $san = Get-Cluster 'PT-Cluster' | Get-Datastore -Name "VNX*" | Sort-Object -Property FreeSpaceGB -Descending
        $Name_Datastore = $san[0]
        return $Name_Datastore
    }
    [string]SelectSwitch($Name_New_VM)
    {
        if ($Name_New_VM.Substring(6,2) -eq 15) { $Network_Name='ProcessTeam' } else {$Network_Name='ProcessTeam-sustaining-678'}
        return $Network_Name
    }
}



#fabryka VMek#####################################################


Class VMachine
{
    [string] $VMName
    [string] $VMName_new
    [string]$Datastore
    [string]$MacAddress 
    [string]$NetworkName
    [string]$Descr1
    [string]$Descr2
    [int] $DiskSize
    VMachine ([string] $VMName_new, [string]$VMname, [string]$Datastore, [string]$MacAddress, [string]$NetworkName,[int] $DiskSize,[string]$Descr1,[string]$Descr2)
    {
        $this.VMName = $VMname
        $this.VMName_new = $VMName_new
        $this.Datastore = $Datastore
        $this.MacAddress = $MacAddress
        $this.NetworkName = $NetworkName
        $this.DiskSize =$DiskSize
        $this.Descr1=$Descr1
        $this.Descr2=$Descr2
    }
    Clone()
    {
    }
}

Class VMachineZero : VMachine
{
    VMachineZero([string] $VMName_new, [string]$VMname, [string]$Datastore, [string]$MacAddress, [string]$NetworkName, [int] $DiskSize,[string]$Descr1,[string]$Descr2) : base ($VMName_new, $VMname, $Datastore, $MacAddress, $NetworkName, $DiskSize,$Descr1,$Descr2)
    {
    }

    [string]Clone()
    {
        New-VM -Name $this.VMname_new -VM $this.VMname -Datastore $this.Datastore -ResourcePool 'PT-Cluster'
        Get-VM  $this.VMName_new |Get-NetworkAdapter| Set-NetworkAdapter -MacAddress $this.MacAddress -NetworkName $this.NetworkName -Confirm:$false
        Get-VM $this.VMname_new | Set-Annotation -CustomAttribute "ticket-id" -Value $this.Descr2
        Get-VM $this.VMname_new | Set-Annotation -CustomAttribute "description" -Value $this.Descr1
        return "Maszyna bez HD"
    }
}

Class VMachineHD : VMachine
{
    VMachineHD([string] $VMName_new, [string]$VMname, [string]$Datastore, [string]$MacAddress, [string]$NetworkName, [int] $DiskSize,[string]$Descr1,[string]$Descr2) : base ($VMName_new, $VMname, $Datastore, $MacAddress, $NetworkName, $DiskSize,$Descr1,$Descr2)
    {
    }

    [string]Clone()
    {
        New-VM -Name $this.VMname_new -VM $this.VMname -Datastore $this.Datastore -ResourcePool 'PT-Cluster'
        Get-VM  $this.VMName_new |Get-NetworkAdapter| Set-NetworkAdapter -MacAddress $this.MacAddress -NetworkName $this.NetworkName -Confirm:$false
        New-HardDisk -VM $this.VMname_new -CapacityGB $this.DiskSize -DiskType Flat -StorageFormat Thin
        Get-VM $this.VMname_new | Set-Annotation -CustomAttribute "ticket-id" -Value $this.Descr2
        Get-VM $this.VMname_new | Set-Annotation -CustomAttribute "description" -Value $this.Descr1
        return "Maszyna z HD"
    }
}

Class VMFactory
{
    static[VMachine[]] $VMachines

    [VMachine] makeMachine([string] $VMName_new, [string]$VMname, [string]$Datastore, [string]$MacAddress, [string]$NetworkName,[int] $DiskSize, [string]$Descr1,[string]$Descr2,[String] $Type)
    {
        return (New-Object -TypeName "$Type" -ArgumentList $VMName_new, $VMname, $Datastore, $MacAddress, $NetworkName, $DiskSize, $Descr1, $Descr2)
    }
}
###########Fasada #####################

Class VMFacade
{
    RemoveAccount($VMName_new,$rootcred)
    {
        Get-VM gklab-79-000  | Invoke-VMScript -GuestCredential $rootcred "/root/rem_acc.sh $VMName_new" -ScriptType Bash
        Start-VM -VM  $VMName_new
        start-sleep -s 120
    }

    SetWindowsVM($VMName_new,$ComputerIP,$AdmCred)
    {
        New-ADComputer -Name $VMName_new -SAMAccountName $VMName_new -ManagedBy "ITP Process Team Admins" -Description 'owner:rkupniex' -DNSHostName $VMName_new.ger.corp.intel.com -Path "OU=Web,OU=Servers,OU=IGK,OU=Customer Labs,OU=Engineering Computing,OU=Resources,DC=ger,DC=corp,DC=intel,DC=com" -Enabled $True -Location "IGK-GDANSK"
        Rename-Computer -ComputerName $ComputerIP -NewName $VMName_new -LocalCredential $AdmCred
        Restart-Computer $ComputerIP -Force -Credential $AdmCred
        $usr=[Security.Principal.WindowsIdentity]::getcurrent().name
        for ($i=1;$i -lt 120;$i++){ start-sleep -s 1
        Write-Progress -Activity "Restart VM" -PercentComplete  $(($i/120)*100) } 
        Add-Computer -ComputerName $ComputerIP -DomainName 'ger.corp.intel.com' -LocalCredential $AdmCred -Force -Credential $usr 
        Restart-Computer -ComputerName $VMName_new -Force -Wait -For WinRM   
    }

    SetLinuxVM($VMName_new,$rootcred)
    {  
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "setenforce 0" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "rm -rfv /etc/ssh/ssh_host* " -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "ssh-keygen -q -A -f /etc/ssh/ " -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "hostnamectl set-hostname '$VMName_new'" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "service vasd stop" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "wget -q --no-check-certificate -O /tmp/krb5_setup.sh http://10.237.156.250/scripts/krb5_setup.sh" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "chmod 0700 /tmp/krb5_setup.sh && /tmp/krb5_setup.sh -s igk" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "keytabs.sh install" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "keytabs.sh" -ScriptType Bash
        Get-VM $VMName_new  | Invoke-VMScript -GuestCredential $rootcred "shutdown -r 1 & " -ScriptType Bash
    }

    SetQBAgent($VMName_new,$ComputerIP,$gkbldcred)
    {
        Invoke-Command -ComputerName $VMName_new -ScriptBlock {  Get-Disk  | Where partitionstyle -eq 'raw' | Initialize-Disk -PartitionStyle GPT -PassThru | New-Partition -AssignDriveLetter -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel "UnEncrypted" -Confirm:$false }
        Copy-Item \\gklab-156-250\stuff\qba2015\qba1 \\$ComputerIP\d$\qba1\ -Force -Recurse
        Invoke-Command -ComputerName $VMName_new -ScriptBlock { icacls D:\qba1 /grant '"GER\gkblditp":(OI)(CI)F' }
        Invoke-Command -ComputerName $VMName_new -ScriptBlock {  New-Service -Name "QuickBuild Build Agent 1" -BinaryPathName "D:\qba1\bin\wrapper-windows-x86-64.exe -s D:\qba1\conf\wrapper.conf" -StartupType Automatic -Credential $gkbldcred -Description "QuickBuild Build Agent 1" -DisplayName "QuickBuild Build Agent 1" } 
        get-service -ComputerName $VMName_new -Name "QuickBuild Build Agent 1" | Start-Service
    }
}

Class VMFacadeMain
{
    Start($VMName_new,$ComputerIP,$AdmCred,$rootcred,$gkbldcred,$HDSize)
    {
        $facade = New-Object VMFacade
        $facade.RemoveAccount($VMName_new,$rootcred)

        $os = (Get-VM -Name $VMName_new).Guest.GuestFamily
        if ($os -eq 'windowsGuest') {$facade.SetWindowsVM($VMName_new,$ComputerIP,$AdmCred)} else {$facade.SetLinuxVM($VMName_new,$rootcred) }
        if ($HDSize -ne 0 -And $os -eq 'windowsGuest') {$facade.SetQBAgent($VMName_new,$ComputerIP,$gkbldcred) }
    }
}


#################################################################################################################

#[VMFactory] $VMFactory = [VMFactory]::new()
#[VMachine] $nowa1 = $VMFactory.makeMachine("gklab","gklab","data","647362","ptnet",10,"VMachineHD")
#$nowa1.Clone()
#[VMachine] $nowa2 = $VMFactory.makeMachine("gklab","gklab","data","647362","ptnet",0,"VMachineZero")
#$nowa2.Clone()

#Get-VM $Name_New_VM | Set-Annotation -CustomAttribute "ticket-id" -Value $VM_ticket_id
#Get-VM $Name_New_VM | Set-Annotation -CustomAttribute "description" -Value $VM_description
#(Get-VM -Name gklab-156-017.ger.corp.intel.com).Guest.GuestFamily

Class Main
{
    
    Start($VMname,$HDsize,$Descr1,$Descr2)
    {
        
        write-host $VMname,$HDsize,$Descr1,$Descr2
        
        
        $netset = New-Object NetSet
        $netid = $netset.EpsilonID()
        $mac = $netset.EpsilonMac($netid)
        $ip = $netset.IpAddress($netid)
        $netset.EpsilonRes($mac,$netid)
        $VMName_new = $netset.HostName($netid)
        $storage = $netset.SelectSAN()
        $switch = $netset.SelectSwitch($VMName_new)


        Write-host $netid,$mac,$ip,$VMName_new,$storage,$switch


        [VMFactory] $VMFactory = [VMFactory]::new()
        if ($HDsize -gt 0) {$type = "VMachineHD"} else {$type = "VMachineZero"}
        [VMachine] $nowa = $VMFactory.makeMachine($VMName_new,$VMname,$storage,$mac,$switch,$HDsize,$Descr1,$Descr2,$type);
        $nowa.Clone()


        $credset = New-Object Credentials
        $mainfacade = New-Object VMFacadeMain
        $mainfacade.Start($VMName_new,$ip,$credset.CredAdmin(),$credset.CredRoot(),$credset.CredGK(),$HDSize)
    }
}



##################### MAIN ##########################################
$start = New-Object Connect
$start.connection()
$vcenter = New-Object VCenter
$main = New-Object Main
$netset = New-Object NetSet

##################### VIEW ##########################################

# Main Window
Add-Type -AssemblyName System.Windows.Forms
$form = New-Object Windows.Forms.Form
$form.Text="Clone VM Script"
$form.Size = New-Object Drawing.Size @(800,600)
$form.StartPosition = "CenterScreen"

# Choice Lists
$list1 = New-Object System.Windows.Forms.ListBox
$list1.Location = New-Object System.Drawing.Size(30,40)
$list1.Size = New-Object System.Drawing.Size(150,250)
Foreach ($element in $vcenter.ListTemplates()){$list1.Items.Add($element)}
$list1.SetSelected(0,$true)
$form.Controls.Add($list1)


#Buttons
$clonebtn1 = New-Object System.Windows.Forms.Button
$clonebtn1.Add_Click({$main.Start($list1.SelectedItem,$outputBox3.Value ,$outputBox1.Text,$outputBox2.Text) })
$clonebtn1.Text = "Clone"
$clonebtn1.Location = New-Object System.Drawing.Size(30,500)
$form.Controls.Add($clonebtn1)

#outboxy
$outputBox1 = New-Object System.Windows.Forms.TextBox 
$outputBox1.Location = New-Object System.Drawing.Size(30,310) 
$outputBox1.Size = New-Object System.Drawing.Size(200,20) 
$outputBox1.MultiLine = $False 
$form.Controls.Add($outputBox1)

$outputBox2 = New-Object System.Windows.Forms.TextBox 
$outputBox2.Location = New-Object System.Drawing.Size(30,360) 
$outputBox2.Size = New-Object System.Drawing.Size(200,20) 
$outputBox2.MultiLine = $False 
$form.Controls.Add($outputBox2)

$outputBox3 = New-Object System.Windows.Forms.NumericUpDown
$outputBox3.Location = New-Object System.Drawing.Size(30,410) 
$outputBox3.Size = New-Object System.Drawing.Size(60,20)  
$form.Controls.Add($outputBox3)

#labels
$Label1 = New-Object System.Windows.Forms.Label
$Label1.Text="List templates:"
$Label1.Location = New-Object System.Drawing.Size(30,20)
$Label1.AutoSize = $True
$form.Controls.Add($Label1)

$Label2 = New-Object System.Windows.Forms.Label
$Label2.Text="Description:"
$Label2.Location = New-Object System.Drawing.Size(30,290)
$Label2.AutoSize = $True
$form.Controls.Add($Label2)

$Label3 = New-Object System.Windows.Forms.Label
$Label3.Text="Ticket:"
$Label3.Location = New-Object System.Drawing.Size(30,340)
$Label3.AutoSize = $True
$form.Controls.Add($Label3)

$Label4 = New-Object System.Windows.Forms.Label
$Label4.Text="HD size (GB):"
$Label4.Location = New-Object System.Drawing.Size(30,390)
$Label4.AutoSize = $True
$form.Controls.Add($Label4)

#outputtext
$outputBox = New-Object System.Windows.Forms.TextBox 
$outputBox.Location = New-Object System.Drawing.Size(300,30) 
$outputBox.Size = New-Object System.Drawing.Size(450,500) 
$outputBox.MultiLine = $True 
$outputBox.ScrollBars = "Vertical" 
$outputBox.ReadOnly = $True
$form.Controls.Add($outputBox)


$drc = $form.ShowDialog()


