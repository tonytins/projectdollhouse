// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from tso on 2014-03-03 06:27:01Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
using System;
using System.ComponentModel;
using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
	using DbLinq.Data.Linq;
	using DbLinq.Vendor;
#endif  // MONO_STRICT
	using System.Data.Linq.Mapping;
using System.Diagnostics;


public partial class DB : DataContext
{
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
	
	
	public DB(string connectionString) : 
			base(connectionString)
	{
		this.OnCreated();
	}
	
	public DB(string connection, MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		this.OnCreated();
	}
	
	public DB(IDbConnection connection, MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		this.OnCreated();
	}
	
	public Table<Account> Accounts
	{
		get
		{
			return this.GetTable<Account>();
		}
	}
	
	public Table<Character> Characters
	{
		get
		{
			return this.GetTable<Character>();
		}
	}
}

#region Start MONO_STRICT
#if MONO_STRICT

public partial class TSo
{
	
	public TSo(IDbConnection connection) : 
			base(connection)
	{
		this.OnCreated();
	}
}
#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT

public partial class DB
{
	
	public DB(IDbConnection connection) : 
			base(connection, new DbLinq.MySql.MySqlVendor())
	{
		this.OnCreated();
	}
	
	public DB(IDbConnection connection, IVendor sqlDialect) : 
			base(connection, sqlDialect)
	{
		this.OnCreated();
	}
	
	public DB(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
			base(connection, mappingSource, sqlDialect)
	{
		this.OnCreated();
	}
}
#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
#endregion

[Table(Name="tso.account")]
public partial class Account : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private int _accountID;
	
	private string _accountName;
	
	private System.Nullable<int> _numCharacters;
	
	private string _password;
	
	private EntitySet<Character> _characters;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnAccountIDChanged();
		
		partial void OnAccountIDChanging(int value);
		
		partial void OnAccountNameChanged();
		
		partial void OnAccountNameChanging(string value);
		
		partial void OnNumCharactersChanged();
		
		partial void OnNumCharactersChanging(System.Nullable<int> value);
		
		partial void OnPasswordChanged();
		
		partial void OnPasswordChanging(string value);
		#endregion
	
	
	public Account()
	{
		_characters = new EntitySet<Character>(new Action<Character>(this.Characters_Attach), new Action<Character>(this.Characters_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_accountID", Name="AccountID", DbType="int(10)", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int AccountID
	{
		get
		{
			return this._accountID;
		}
		set
		{
			if ((_accountID != value))
			{
				this.OnAccountIDChanging(value);
				this.SendPropertyChanging();
				this._accountID = value;
				this.SendPropertyChanged("AccountID");
				this.OnAccountIDChanged();
			}
		}
	}
	
	[Column(Storage="_accountName", Name="AccountName", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string AccountName
	{
		get
		{
			return this._accountName;
		}
		set
		{
			if (((_accountName == value) 
						== false))
			{
				this.OnAccountNameChanging(value);
				this.SendPropertyChanging();
				this._accountName = value;
				this.SendPropertyChanged("AccountName");
				this.OnAccountNameChanged();
			}
		}
	}
	
	[Column(Storage="_numCharacters", Name="NumCharacters", DbType="int", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public System.Nullable<int> NumCharacters
	{
		get
		{
			return this._numCharacters;
		}
		set
		{
			if ((_numCharacters != value))
			{
				this.OnNumCharactersChanging(value);
				this.SendPropertyChanging();
				this._numCharacters = value;
				this.SendPropertyChanged("NumCharacters");
				this.OnNumCharactersChanged();
			}
		}
	}
	
	[Column(Storage="_password", Name="Password", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string Password
	{
		get
		{
			return this._password;
		}
		set
		{
			if (((_password == value) 
						== false))
			{
				this.OnPasswordChanging(value);
				this.SendPropertyChanging();
				this._password = value;
				this.SendPropertyChanged("Password");
				this.OnPasswordChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_characters", OtherKey="AccountID", ThisKey="AccountID", Name="FK_character_account")]
	[DebuggerNonUserCode()]
	public EntitySet<Character> Characters
	{
		get
		{
			return this._characters;
		}
		set
		{
			this._characters = value;
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void Characters_Attach(Character entity)
	{
		this.SendPropertyChanging();
		entity.Account = this;
	}
	
	private void Characters_Detach(Character entity)
	{
		this.SendPropertyChanging();
		entity.Account = null;
	}
	#endregion
}

[Table(Name="tso.character")]
public partial class Character : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private int _accountID;
	
	private int _appearanceType;
	
	private long _bodyOutfitID;
	
	private int _characterID;
	
	private string _city;
	
	private string _cityIp;
	
	private System.Nullable<long> _cityMap;
	
	private string _cityName;
	
	private System.Nullable<int> _cityPort;
	
	private System.Nullable<long> _cityThumb;
	
	private string _description;
	
	private System.Guid _guid;
	
	private long _headOutfitID;
	
	private string _lastCached;
	
	private string _name;
	
	private string _sex;
	
	private EntityRef<Account> _account = new EntityRef<Account>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnAccountIDChanged();
		
		partial void OnAccountIDChanging(int value);
		
		partial void OnAppearanceTypeChanged();
		
		partial void OnAppearanceTypeChanging(int value);
		
		partial void OnBodyOutfitIDChanged();
		
		partial void OnBodyOutfitIDChanging(long value);
		
		partial void OnCharacterIDChanged();
		
		partial void OnCharacterIDChanging(int value);
		
		partial void OnCityChanged();
		
		partial void OnCityChanging(string value);
		
		partial void OnCityIpChanged();
		
		partial void OnCityIpChanging(string value);
		
		partial void OnCityMapChanged();
		
		partial void OnCityMapChanging(System.Nullable<long> value);
		
		partial void OnCityNameChanged();
		
		partial void OnCityNameChanging(string value);
		
		partial void OnCityPortChanged();
		
		partial void OnCityPortChanging(System.Nullable<int> value);
		
		partial void OnCityThumbChanged();
		
		partial void OnCityThumbChanging(System.Nullable<long> value);
		
		partial void OnDescriptionChanged();
		
		partial void OnDescriptionChanging(string value);
		
		partial void OnGUIDChanged();
		
		partial void OnGUIDChanging(System.Guid value);
		
		partial void OnHeadOutfitIDChanged();
		
		partial void OnHeadOutfitIDChanging(long value);
		
		partial void OnLastCachedChanged();
		
		partial void OnLastCachedChanging(string value);
		
		partial void OnNameChanged();
		
		partial void OnNameChanging(string value);
		
		partial void OnSexChanged();
		
		partial void OnSexChanging(string value);
		#endregion
	
	
	public Character()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_accountID", Name="AccountID", DbType="int(10)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int AccountID
	{
		get
		{
			return this._accountID;
		}
		set
		{
			if ((_accountID != value))
			{
				if (_account.HasLoadedOrAssignedValue)
				{
					throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				}
				this.OnAccountIDChanging(value);
				this.SendPropertyChanging();
				this._accountID = value;
				this.SendPropertyChanged("AccountID");
				this.OnAccountIDChanged();
			}
		}
	}
	
	[Column(Storage="_appearanceType", Name="AppearanceType", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int AppearanceType
	{
		get
		{
			return this._appearanceType;
		}
		set
		{
			if ((_appearanceType != value))
			{
				this.OnAppearanceTypeChanging(value);
				this.SendPropertyChanging();
				this._appearanceType = value;
				this.SendPropertyChanged("AppearanceType");
				this.OnAppearanceTypeChanged();
			}
		}
	}
	
	[Column(Storage="_bodyOutfitID", Name="BodyOutfitID", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public long BodyOutfitID
	{
		get
		{
			return this._bodyOutfitID;
		}
		set
		{
			if ((_bodyOutfitID != value))
			{
				this.OnBodyOutfitIDChanging(value);
				this.SendPropertyChanging();
				this._bodyOutfitID = value;
				this.SendPropertyChanged("BodyOutfitID");
				this.OnBodyOutfitIDChanged();
			}
		}
	}
	
	[Column(Storage="_characterID", Name="CharacterID", DbType="int(10)", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int CharacterID
	{
		get
		{
			return this._characterID;
		}
		set
		{
			if ((_characterID != value))
			{
				this.OnCharacterIDChanging(value);
				this.SendPropertyChanging();
				this._characterID = value;
				this.SendPropertyChanged("CharacterID");
				this.OnCharacterIDChanged();
			}
		}
	}
	
	[Column(Storage="_city", Name="City", DbType="varchar(50)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string City
	{
		get
		{
			return this._city;
		}
		set
		{
			if (((_city == value) 
						== false))
			{
				this.OnCityChanging(value);
				this.SendPropertyChanging();
				this._city = value;
				this.SendPropertyChanged("City");
				this.OnCityChanged();
			}
		}
	}
	
	[Column(Storage="_cityIp", Name="CityIP", DbType="varchar(16)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string CityIp
	{
		get
		{
			return this._cityIp;
		}
		set
		{
			if (((_cityIp == value) 
						== false))
			{
				this.OnCityIpChanging(value);
				this.SendPropertyChanging();
				this._cityIp = value;
				this.SendPropertyChanged("CityIp");
				this.OnCityIpChanged();
			}
		}
	}
	
	[Column(Storage="_cityMap", Name="CityMap", DbType="bigint(20)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public System.Nullable<long> CityMap
	{
		get
		{
			return this._cityMap;
		}
		set
		{
			if ((_cityMap != value))
			{
				this.OnCityMapChanging(value);
				this.SendPropertyChanging();
				this._cityMap = value;
				this.SendPropertyChanged("CityMap");
				this.OnCityMapChanged();
			}
		}
	}
	
	[Column(Storage="_cityName", Name="CityName", DbType="varchar(65)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string CityName
	{
		get
		{
			return this._cityName;
		}
		set
		{
			if (((_cityName == value) 
						== false))
			{
				this.OnCityNameChanging(value);
				this.SendPropertyChanging();
				this._cityName = value;
				this.SendPropertyChanged("CityName");
				this.OnCityNameChanged();
			}
		}
	}
	
	[Column(Storage="_cityPort", Name="CityPort", DbType="int", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public System.Nullable<int> CityPort
	{
		get
		{
			return this._cityPort;
		}
		set
		{
			if ((_cityPort != value))
			{
				this.OnCityPortChanging(value);
				this.SendPropertyChanging();
				this._cityPort = value;
				this.SendPropertyChanged("CityPort");
				this.OnCityPortChanged();
			}
		}
	}
	
	[Column(Storage="_cityThumb", Name="CityThumb", DbType="bigint(20)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public System.Nullable<long> CityThumb
	{
		get
		{
			return this._cityThumb;
		}
		set
		{
			if ((_cityThumb != value))
			{
				this.OnCityThumbChanging(value);
				this.SendPropertyChanging();
				this._cityThumb = value;
				this.SendPropertyChanged("CityThumb");
				this.OnCityThumbChanged();
			}
		}
	}
	
	[Column(Storage="_description", Name="Description", DbType="varchar(400)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string Description
	{
		get
		{
			return this._description;
		}
		set
		{
			if (((_description == value) 
						== false))
			{
				this.OnDescriptionChanging(value);
				this.SendPropertyChanging();
				this._description = value;
				this.SendPropertyChanged("Description");
				this.OnDescriptionChanged();
			}
		}
	}
	
	[Column(Storage="_guid", Name="GUID", DbType="varchar(36)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.Guid GUID
	{
		get
		{
			return this._guid;
		}
		set
		{
			if ((_guid != value))
			{
				this.OnGUIDChanging(value);
				this.SendPropertyChanging();
				this._guid = value;
				this.SendPropertyChanged("GUID");
				this.OnGUIDChanged();
			}
		}
	}
	
	[Column(Storage="_headOutfitID", Name="HeadOutfitID", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public long HeadOutfitID
	{
		get
		{
			return this._headOutfitID;
		}
		set
		{
			if ((_headOutfitID != value))
			{
				this.OnHeadOutfitIDChanging(value);
				this.SendPropertyChanging();
				this._headOutfitID = value;
				this.SendPropertyChanged("HeadOutfitID");
				this.OnHeadOutfitIDChanged();
			}
		}
	}
	
	[Column(Storage="_lastCached", Name="LastCached", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string LastCached
	{
		get
		{
			return this._lastCached;
		}
		set
		{
			if (((_lastCached == value) 
						== false))
			{
				this.OnLastCachedChanging(value);
				this.SendPropertyChanging();
				this._lastCached = value;
				this.SendPropertyChanged("LastCached");
				this.OnLastCachedChanged();
			}
		}
	}
	
	[Column(Storage="_name", Name="Name", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string Name
	{
		get
		{
			return this._name;
		}
		set
		{
			if (((_name == value) 
						== false))
			{
				this.OnNameChanging(value);
				this.SendPropertyChanging();
				this._name = value;
				this.SendPropertyChanged("Name");
				this.OnNameChanged();
			}
		}
	}
	
	[Column(Storage="_sex", Name="Sex", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string Sex
	{
		get
		{
			return this._sex;
		}
		set
		{
			if (((_sex == value) 
						== false))
			{
				this.OnSexChanging(value);
				this.SendPropertyChanging();
				this._sex = value;
				this.SendPropertyChanged("Sex");
				this.OnSexChanged();
			}
		}
	}
	
	#region Parents
	[Association(Storage="_account", OtherKey="AccountID", ThisKey="AccountID", Name="FK_character_account", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public Account Account
	{
		get
		{
			return this._account.Entity;
		}
		set
		{
			if (((this._account.Entity == value) 
						== false))
			{
				if ((this._account.Entity != null))
				{
					Account previousAccount = this._account.Entity;
					this._account.Entity = null;
					previousAccount.Characters.Remove(this);
				}
				this._account.Entity = value;
				if ((value != null))
				{
					value.Characters.Add(this);
					_accountID = value.AccountID;
				}
				else
				{
					_accountID = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}
