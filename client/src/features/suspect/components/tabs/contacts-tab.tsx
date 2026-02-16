import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { MapPin, Phone, Edit2Icon, Trash2Icon } from 'lucide-react';
import { SuspectDetails, Address, Contact } from '../../types/suspect';
import { InfoItem } from '../info-item';

interface ContactsTabProps {
  suspect: SuspectDetails;
  onAddAddress: () => void;
  onEditAddress: (address: Address) => void;
  onDeleteAddress: (address: Address) => void;
  onAddContact: () => void;
  onEditContact: (contact: Contact) => void;
  onDeleteContact: (contact: Contact) => void;
}

export function ContactsTab({
  suspect,
  onAddAddress,
  onEditAddress,
  onDeleteAddress,
  onAddContact,
  onEditContact,
  onDeleteContact
}: ContactsTabProps) {
  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <MapPin className="h-5 w-5" />
              العناوين
            </CardTitle>
            <Button variant="outline" onClick={onAddAddress}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent>
          {suspect.addresses && suspect.addresses.length > 0 ? (
            <div className="space-y-4">
              {suspect.addresses.map((address) => (
                <div key={address.id} className="rounded-lg border p-4 bg-gray-100 dark:bg-zinc-900">
                  <div className='relative top-0 right-0 flex justify-end gap-1 mb-2'>
                    <Button variant="outline" size="sm" onClick={() => onEditAddress(address)}>
                      <Edit2Icon className="h-4 w-4" />
                    </Button>
                    <Button variant="outline" size="sm" onClick={() => onDeleteAddress(address)}>
                      <Trash2Icon className="h-4 w-4" color='red' />
                    </Button>
                  </div>
                  <div className="mb-2">
                    <Badge variant="outline">{address.typeName}</Badge>
                  </div>
                  <div className="grid gap-2 md:grid-cols-2">
                    <InfoItem label="المدينة" value={address.city} />
                    <InfoItem label="الحي" value={address.district} />
                    <InfoItem label="التفاصيل" value={address.details} />
                    <InfoItem label="الإحداثيات" value={address.gpsCoordinates} />
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-muted-foreground text-center py-8">لا توجد عناوين مسجلة</p>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <Phone className="h-5 w-5" />
              معلومات الاتصال
            </CardTitle>
            <Button variant="outline" onClick={onAddContact}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent>
          {suspect.contacts && suspect.contacts.length > 0 ? (
            <div className="space-y-3">
              {suspect.contacts.map((contact) => (
                <div key={contact.id} className="rounded-lg border p-3 space-y-3 bg-gray-100 dark:bg-zinc-900">
                  <div className='relative top-0 right-0 flex justify-end gap-1'>
                    <Button variant="outline" size="sm" onClick={() => onEditContact(contact)}>
                      <Edit2Icon className="h-4 w-4" />
                    </Button>
                    <Button variant="outline" size="sm" onClick={() => onDeleteContact(contact)}>
                      <Trash2Icon className="h-4 w-4" color='red' />
                    </Button>
                  </div>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3">
                      <Badge variant="secondary">{contact.typeName}</Badge>
                      <span className="font-medium">{contact.value}</span>
                    </div>
                    {contact.ownerName && (
                      <span className="text-muted-foreground text-sm">{contact.ownerName}</span>
                    )}
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-muted-foreground text-center py-8">لا توجد معلومات اتصال مسجلة</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
